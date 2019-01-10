using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class WallPaper : ITlType, IEquatable<WallPaper>, IComparable<WallPaper>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0xccb03657;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public int Id { get; }
            public string Title { get; }
            public Arr<T.PhotoSize> Sizes { get; }
            public int Color { get; }
            
            public Tag(
                int id,
                Some<string> title,
                Some<Arr<T.PhotoSize>> sizes,
                int color
            ) {
                Id = id;
                Title = title;
                Sizes = sizes;
                Color = color;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Id, bw, WriteInt);
                Write(Title, bw, WriteString);
                Write(Sizes, bw, WriteVector<T.PhotoSize>(WriteSerializable));
                Write(Color, bw, WriteInt);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var id = Read(br, ReadInt);
                var title = Read(br, ReadString);
                var sizes = Read(br, ReadVector(T.PhotoSize.Deserialize));
                var color = Read(br, ReadInt);
                return new Tag(id, title, sizes, color);
            }
        }

        public sealed class SolidTag : Record<SolidTag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0x63117f24;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public int Id { get; }
            public string Title { get; }
            public int BgColor { get; }
            public int Color { get; }
            
            public SolidTag(
                int id,
                Some<string> title,
                int bgColor,
                int color
            ) {
                Id = id;
                Title = title;
                BgColor = bgColor;
                Color = color;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Id, bw, WriteInt);
                Write(Title, bw, WriteString);
                Write(BgColor, bw, WriteInt);
                Write(Color, bw, WriteInt);
            }
            
            internal static SolidTag DeserializeTag(BinaryReader br)
            {
                var id = Read(br, ReadInt);
                var title = Read(br, ReadString);
                var bgColor = Read(br, ReadInt);
                var color = Read(br, ReadInt);
                return new SolidTag(id, title, bgColor, color);
            }
        }

        readonly ITlTypeTag _tag;
        WallPaper(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator WallPaper(Tag tag) => new WallPaper(tag);
        public static explicit operator WallPaper(SolidTag tag) => new WallPaper(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static WallPaper Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (WallPaper) Tag.DeserializeTag(br);
                case SolidTag.TypeNumber: return (WallPaper) SolidTag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { Tag.TypeNumber, SolidTag.TypeNumber });
            }
        }

        public T Match<T>(
            Func<T> _,
            Func<Tag, T> tag = null,
            Func<SolidTag, T> solidTag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case Tag x when tag != null: return tag(x);
                case SolidTag x when solidTag != null: return solidTag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<Tag, T> tag,
            Func<SolidTag, T> solidTag
        ) => Match(
            () => throw new Exception("WTF"),
            tag ?? throw new ArgumentNullException(nameof(tag)),
            solidTag ?? throw new ArgumentNullException(nameof(solidTag))
        );

        public bool Equals(WallPaper other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is WallPaper x && Equals(x);
        public static bool operator ==(WallPaper a, WallPaper b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(WallPaper a, WallPaper b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                case SolidTag _: return 1;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(WallPaper other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is WallPaper x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(WallPaper a, WallPaper b) => a.CompareTo(b) <= 0;
        public static bool operator <(WallPaper a, WallPaper b) => a.CompareTo(b) < 0;
        public static bool operator >(WallPaper a, WallPaper b) => a.CompareTo(b) > 0;
        public static bool operator >=(WallPaper a, WallPaper b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}