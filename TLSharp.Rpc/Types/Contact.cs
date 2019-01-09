using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class Contact : ITlType, IEquatable<Contact>, IComparable<Contact>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            uint ITlTypeTag.TypeNumber => 0xf911c994;
            
            public int UserId { get; }
            public bool Mutual { get; }
            
            public Tag(
                int userId,
                bool mutual
            ) {
                UserId = userId;
                Mutual = mutual;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(UserId, bw, WriteInt);
                Write(Mutual, bw, WriteBool);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var userId = Read(br, ReadInt);
                var mutual = Read(br, ReadBool);
                return new Tag(userId, mutual);
            }
        }

        readonly ITlTypeTag _tag;
        Contact(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator Contact(Tag tag) => new Contact(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static Contact Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case 0xf911c994: return (Contact) Tag.DeserializeTag(br);
                default: throw TlTransportException.UnexpectedTypeNumber(actual: typeNumber, expected: new uint[] { 0xf911c994 });
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

        public bool Equals(Contact other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is Contact x && Equals(x);
        public static bool operator ==(Contact a, Contact b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(Contact a, Contact b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(Contact other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is Contact x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(Contact a, Contact b) => a.CompareTo(b) <= 0;
        public static bool operator <(Contact a, Contact b) => a.CompareTo(b) < 0;
        public static bool operator >(Contact a, Contact b) => a.CompareTo(b) > 0;
        public static bool operator >=(Contact a, Contact b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}