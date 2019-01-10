using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class ServerDhParams : ITlType, IEquatable<ServerDhParams>, IComparable<ServerDhParams>, IComparable
    {
        public sealed class FailTag : Record<FailTag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0x79cb045d;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public Int128 Nonce { get; }
            public Int128 ServerNonce { get; }
            public Int128 NewNonceHash { get; }
            
            public FailTag(
                Int128 nonce,
                Int128 serverNonce,
                Int128 newNonceHash
            ) {
                Nonce = nonce;
                ServerNonce = serverNonce;
                NewNonceHash = newNonceHash;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Nonce, bw, WriteInt128);
                Write(ServerNonce, bw, WriteInt128);
                Write(NewNonceHash, bw, WriteInt128);
            }
            
            internal static FailTag DeserializeTag(BinaryReader br)
            {
                var nonce = Read(br, ReadInt128);
                var serverNonce = Read(br, ReadInt128);
                var newNonceHash = Read(br, ReadInt128);
                return new FailTag(nonce, serverNonce, newNonceHash);
            }
        }

        public sealed class OkTag : Record<OkTag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0xd0e8075c;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public Int128 Nonce { get; }
            public Int128 ServerNonce { get; }
            public Arr<byte> EncryptedAnswer { get; }
            
            public OkTag(
                Int128 nonce,
                Int128 serverNonce,
                Some<Arr<byte>> encryptedAnswer
            ) {
                Nonce = nonce;
                ServerNonce = serverNonce;
                EncryptedAnswer = encryptedAnswer;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Nonce, bw, WriteInt128);
                Write(ServerNonce, bw, WriteInt128);
                Write(EncryptedAnswer, bw, WriteBytes);
            }
            
            internal static OkTag DeserializeTag(BinaryReader br)
            {
                var nonce = Read(br, ReadInt128);
                var serverNonce = Read(br, ReadInt128);
                var encryptedAnswer = Read(br, ReadBytes);
                return new OkTag(nonce, serverNonce, encryptedAnswer);
            }
        }

        readonly ITlTypeTag _tag;
        ServerDhParams(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator ServerDhParams(FailTag tag) => new ServerDhParams(tag);
        public static explicit operator ServerDhParams(OkTag tag) => new ServerDhParams(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static ServerDhParams Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case FailTag.TypeNumber: return (ServerDhParams) FailTag.DeserializeTag(br);
                case OkTag.TypeNumber: return (ServerDhParams) OkTag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { FailTag.TypeNumber, OkTag.TypeNumber });
            }
        }

        public T Match<T>(
            Func<T> _,
            Func<FailTag, T> failTag = null,
            Func<OkTag, T> okTag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case FailTag x when failTag != null: return failTag(x);
                case OkTag x when okTag != null: return okTag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<FailTag, T> failTag,
            Func<OkTag, T> okTag
        ) => Match(
            () => throw new Exception("WTF"),
            failTag ?? throw new ArgumentNullException(nameof(failTag)),
            okTag ?? throw new ArgumentNullException(nameof(okTag))
        );

        public bool Equals(ServerDhParams other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is ServerDhParams x && Equals(x);
        public static bool operator ==(ServerDhParams a, ServerDhParams b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(ServerDhParams a, ServerDhParams b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case FailTag _: return 0;
                case OkTag _: return 1;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(ServerDhParams other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is ServerDhParams x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(ServerDhParams a, ServerDhParams b) => a.CompareTo(b) <= 0;
        public static bool operator <(ServerDhParams a, ServerDhParams b) => a.CompareTo(b) < 0;
        public static bool operator >(ServerDhParams a, ServerDhParams b) => a.CompareTo(b) > 0;
        public static bool operator >=(ServerDhParams a, ServerDhParams b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}