using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Messages
{
    public sealed class ArchivedStickers : ITlType, IEquatable<ArchivedStickers>, IComparable<ArchivedStickers>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0x4fcba9c8;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly int Count;
            public readonly Arr<T.StickerSetCovered> Sets;
            
            public Tag(
                int count,
                Some<Arr<T.StickerSetCovered>> sets
            ) {
                Count = count;
                Sets = sets;
            }
            
            (int, Arr<T.StickerSetCovered>) CmpTuple =>
                (Count, Sets);

            public bool Equals(Tag other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpTuple == other.CmpTuple);
            public override bool Equals(object other) => other is Tag x && Equals(x);
            public static bool operator ==(Tag x, Tag y) => x?.Equals(y) ?? ReferenceEquals(y, null);
            public static bool operator !=(Tag x, Tag y) => !(x == y);

            public int CompareTo(Tag other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpTuple.CompareTo(other.CmpTuple);
            int IComparable.CompareTo(object other) => other is Tag x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
            public static bool operator <=(Tag x, Tag y) => x.CompareTo(y) <= 0;
            public static bool operator <(Tag x, Tag y) => x.CompareTo(y) < 0;
            public static bool operator >(Tag x, Tag y) => x.CompareTo(y) > 0;
            public static bool operator >=(Tag x, Tag y) => x.CompareTo(y) >= 0;

            public override int GetHashCode() => CmpTuple.GetHashCode();

            public override string ToString() => $"(Count: {Count}, Sets: {Sets})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Count, bw, WriteInt);
                Write(Sets, bw, WriteVector<T.StickerSetCovered>(WriteSerializable));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var count = Read(br, ReadInt);
                var sets = Read(br, ReadVector(T.StickerSetCovered.Deserialize));
                return new Tag(count, sets);
            }
        }

        readonly ITlTypeTag _tag;
        ArchivedStickers(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator ArchivedStickers(Tag tag) => new ArchivedStickers(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static ArchivedStickers Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (ArchivedStickers) Tag.DeserializeTag(br);
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

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public bool Equals(ArchivedStickers other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is ArchivedStickers x && Equals(x);
        public static bool operator ==(ArchivedStickers x, ArchivedStickers y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(ArchivedStickers x, ArchivedStickers y) => !(x == y);

        public int CompareTo(ArchivedStickers other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is ArchivedStickers x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(ArchivedStickers x, ArchivedStickers y) => x.CompareTo(y) <= 0;
        public static bool operator <(ArchivedStickers x, ArchivedStickers y) => x.CompareTo(y) < 0;
        public static bool operator >(ArchivedStickers x, ArchivedStickers y) => x.CompareTo(y) > 0;
        public static bool operator >=(ArchivedStickers x, ArchivedStickers y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"ArchivedStickers.{_tag.GetType().Name}{_tag}";
    }
}