using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Messages
{
    public sealed class HighScores : ITlType, IEquatable<HighScores>, IComparable<HighScores>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            uint ITlTypeTag.TypeNumber => 0x9a3bfd99;
            
            public Arr<T.HighScore> Scores { get; }
            public Arr<T.User> Users { get; }
            
            public Tag(
                Some<Arr<T.HighScore>> scores,
                Some<Arr<T.User>> users
            ) {
                Scores = scores;
                Users = users;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Scores, bw, WriteVector<T.HighScore>(WriteSerializable));
                Write(Users, bw, WriteVector<T.User>(WriteSerializable));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var scores = Read(br, ReadVector(T.HighScore.Deserialize));
                var users = Read(br, ReadVector(T.User.Deserialize));
                return new Tag(scores, users);
            }
        }

        readonly ITlTypeTag _tag;
        HighScores(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator HighScores(Tag tag) => new HighScores(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static HighScores Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case 0x9a3bfd99: return (HighScores) Tag.DeserializeTag(br);
                default: throw TlTransportException.UnexpectedTypeNumber(actual: typeNumber, expected: new uint[] { 0x9a3bfd99 });
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

        public bool Equals(HighScores other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is HighScores x && Equals(x);
        public static bool operator ==(HighScores a, HighScores b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(HighScores a, HighScores b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(HighScores other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is HighScores x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(HighScores a, HighScores b) => a.CompareTo(b) <= 0;
        public static bool operator <(HighScores a, HighScores b) => a.CompareTo(b) < 0;
        public static bool operator >(HighScores a, HighScores b) => a.CompareTo(b) > 0;
        public static bool operator >=(HighScores a, HighScores b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}