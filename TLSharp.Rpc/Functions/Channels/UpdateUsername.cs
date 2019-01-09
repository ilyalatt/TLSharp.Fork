using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Functions.Channels
{
    public sealed class UpdateUsername : Record<UpdateUsername>, ITlFunc<bool>
    {
        public T.InputChannel Channel { get; }
        public string Username { get; }
        
        public UpdateUsername(
            Some<T.InputChannel> channel,
            Some<string> username
        ) {
            Channel = channel;
            Username = username;
        }
        
        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, 0x3514b3de);
            Write(Channel, bw, WriteSerializable);
            Write(Username, bw, WriteString);
        }
        
        bool ITlFunc<bool>.DeserializeResult(BinaryReader br) =>
            Read(br, ReadBool);
    }
}