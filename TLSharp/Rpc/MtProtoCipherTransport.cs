using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using TLSharp.Rpc;
using TLSharp.Utils;

namespace TLSharp.Rpc
{
    class MtProtoCipherTransport : IDisposable
    {
        readonly TcpTransport _transport;
        readonly Session _session;

        public MtProtoCipherTransport(TcpTransport transport, Session session)
        {
            _transport = transport;
            _session = session;
        }

        public void Dispose() => _transport.Dispose();


        int GetSeqNum(bool inc) => inc ? _session.Sequence++ * 2 + 1 : _session.Sequence * 2;


        async Task SendMsgBody(long messageId, bool incSeqNum, byte[] msg)
        {
            var msgSeqNum = GetSeqNum(incSeqNum);
            _session.Save();

            var plainText = BtHelpers.UsingMemBinWriter(bw =>
            {
                bw.Write(_session.Salt);
                bw.Write(_session.Id);
                bw.Write(messageId);
                bw.Write(msgSeqNum);

                bw.Write(msg.Length);
                bw.Write(msg);
            });

            var msgKey = Helpers.CalcMsgKey(plainText);
            var cipherText = Aes.EncryptAES(
                Helpers.CalcKey(_session.AuthKey.Key.ToArray(), msgKey, true),
                plainText
            );

            await BtHelpers.UsingMemBinWriter(bw =>
            {
                bw.Write(_session.AuthKey.KeyId);
                bw.Write(msgKey);
                bw.Write(cipherText);
            }).Apply(_transport.Send);
        }

        public async Task Send(long messageId, bool incSeqNum, ITlSerializable dto)
        {
            var bts = BtHelpers.UsingMemBinWriter(dto.Serialize);
            await SendMsgBody(messageId, incSeqNum, bts);
        }


        async Task<byte[]> ReceivePlainText()
        {
            var (tcpSeqNum, body) = await _transport.Receive();

            const uint sessionClosed = 0xfffffe6c; // i dunno why
            if (body.Length == 4 && BitConverter.ToUInt32(body, 0) == sessionClosed)
            {
                throw new InvalidOperationException("The session is closed.");
            }
            // TODO: include the message in an exception
            // if (body.Length < 8) throw new InvalidOperationException("Can't decode packet");

            return body.Apply(BtHelpers.Deserialize(br =>
            {
                var authKeyId = br.ReadUInt64(); // TODO: check auth key id
                var msgKey = br.ReadBytes(16); // TODO: check msg_key correctness
                var keyData = Helpers.CalcKey(_session.AuthKey.Key.ToArray(), msgKey, false);

                var cipherTextLen = (int) (br.BaseStream.Length - br.BaseStream.Position);
                var cipherText = br.ReadBytes(cipherTextLen);
                var plainText = Aes.DecryptAES(keyData, cipherText);

                return plainText;
            }));
        }

        public async Task<BinaryReader> Receive()
        {
            var plainText = await ReceivePlainText();
            return plainText.Apply(BtHelpers.Deserialize(br =>
            {
                var remoteSalt = br.ReadUInt64();
                var remoteSessionId = br.ReadUInt64();

                return br;
            }));
        }
    }
}