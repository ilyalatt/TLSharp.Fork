using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Help
{
    public sealed class InviteText : ITlType, IEquatable<InviteText>, IComparable<InviteText>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0x18cb9f78;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public string Message { get; }
            
            public Tag(
                Some<string> message
            ) {
                Message = message;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Message, bw, WriteString);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var message = Read(br, ReadString);
                return new Tag(message);
            }
        }

        readonly ITlTypeTag _tag;
        InviteText(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator InviteText(Tag tag) => new InviteText(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static InviteText Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (InviteText) Tag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { Tag.TypeNumber });
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

        public bool Equals(InviteText other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is InviteText x && Equals(x);
        public static bool operator ==(InviteText a, InviteText b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(InviteText a, InviteText b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(InviteText other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is InviteText x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(InviteText a, InviteText b) => a.CompareTo(b) <= 0;
        public static bool operator <(InviteText a, InviteText b) => a.CompareTo(b) < 0;
        public static bool operator >(InviteText a, InviteText b) => a.CompareTo(b) > 0;
        public static bool operator >=(InviteText a, InviteText b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}