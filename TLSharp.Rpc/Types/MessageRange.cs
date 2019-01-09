using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class MessageRange : ITlType, IEquatable<MessageRange>, IComparable<MessageRange>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            uint ITlTypeTag.TypeNumber => 0x0ae30253;
            
            public int MinId { get; }
            public int MaxId { get; }
            
            public Tag(
                int minId,
                int maxId
            ) {
                MinId = minId;
                MaxId = maxId;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(MinId, bw, WriteInt);
                Write(MaxId, bw, WriteInt);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var minId = Read(br, ReadInt);
                var maxId = Read(br, ReadInt);
                return new Tag(minId, maxId);
            }
        }

        readonly ITlTypeTag _tag;
        MessageRange(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator MessageRange(Tag tag) => new MessageRange(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static MessageRange Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case 0x0ae30253: return (MessageRange) Tag.DeserializeTag(br);
                default: throw TlTransportException.UnexpectedTypeNumber(actual: typeNumber, expected: new uint[] { 0x0ae30253 });
            }
        }

        T Match<T>(
            Func<T> _,
            Func<Tag, T> tag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case Tag x when tag != null: return tag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<Tag, T> tag
        ) => Match(
            () => throw new Exception("WTF"),
            tag ?? throw new ArgumentNullException(nameof(tag))
        );

        public bool Equals(MessageRange other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is MessageRange x && Equals(x);
        public static bool operator ==(MessageRange a, MessageRange b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(MessageRange a, MessageRange b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(MessageRange other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is MessageRange x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(MessageRange a, MessageRange b) => a.CompareTo(b) <= 0;
        public static bool operator <(MessageRange a, MessageRange b) => a.CompareTo(b) < 0;
        public static bool operator >(MessageRange a, MessageRange b) => a.CompareTo(b) > 0;
        public static bool operator >=(MessageRange a, MessageRange b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}