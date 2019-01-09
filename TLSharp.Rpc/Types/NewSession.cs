using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class NewSession : ITlType, IEquatable<NewSession>, IComparable<NewSession>, IComparable
    {
        public sealed class CreatedTag : Record<CreatedTag>, ITlTypeTag
        {
            uint ITlTypeTag.TypeNumber => 0x9ec20908;
            
            public long FirstMsgId { get; }
            public long UniqueId { get; }
            public long ServerSalt { get; }
            
            public CreatedTag(
                long firstMsgId,
                long uniqueId,
                long serverSalt
            ) {
                FirstMsgId = firstMsgId;
                UniqueId = uniqueId;
                ServerSalt = serverSalt;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(FirstMsgId, bw, WriteLong);
                Write(UniqueId, bw, WriteLong);
                Write(ServerSalt, bw, WriteLong);
            }
            
            internal static CreatedTag DeserializeTag(BinaryReader br)
            {
                var firstMsgId = Read(br, ReadLong);
                var uniqueId = Read(br, ReadLong);
                var serverSalt = Read(br, ReadLong);
                return new CreatedTag(firstMsgId, uniqueId, serverSalt);
            }
        }

        readonly ITlTypeTag _tag;
        NewSession(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator NewSession(CreatedTag tag) => new NewSession(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static NewSession Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case 0x9ec20908: return (NewSession) CreatedTag.DeserializeTag(br);
                default: throw TlTransportException.UnexpectedTypeNumber(actual: typeNumber, expected: new uint[] { 0x9ec20908 });
            }
        }

        T Match<T>(
            Func<T> _,
            Func<CreatedTag, T> createdTag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case CreatedTag x when createdTag != null: return createdTag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<CreatedTag, T> createdTag
        ) => Match(
            () => throw new Exception("WTF"),
            createdTag ?? throw new ArgumentNullException(nameof(createdTag))
        );

        public bool Equals(NewSession other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is NewSession x && Equals(x);
        public static bool operator ==(NewSession a, NewSession b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(NewSession a, NewSession b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case CreatedTag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(NewSession other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is NewSession x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(NewSession a, NewSession b) => a.CompareTo(b) <= 0;
        public static bool operator <(NewSession a, NewSession b) => a.CompareTo(b) < 0;
        public static bool operator >(NewSession a, NewSession b) => a.CompareTo(b) > 0;
        public static bool operator >=(NewSession a, NewSession b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}