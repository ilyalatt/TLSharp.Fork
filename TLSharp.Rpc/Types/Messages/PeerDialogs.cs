using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Messages
{
    public sealed class PeerDialogs : ITlType, IEquatable<PeerDialogs>, IComparable<PeerDialogs>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0x3371c354;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public Arr<T.Dialog> Dialogs { get; }
            public Arr<T.Message> Messages { get; }
            public Arr<T.Chat> Chats { get; }
            public Arr<T.User> Users { get; }
            public T.Updates.State State { get; }
            
            public Tag(
                Some<Arr<T.Dialog>> dialogs,
                Some<Arr<T.Message>> messages,
                Some<Arr<T.Chat>> chats,
                Some<Arr<T.User>> users,
                Some<T.Updates.State> state
            ) {
                Dialogs = dialogs;
                Messages = messages;
                Chats = chats;
                Users = users;
                State = state;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Dialogs, bw, WriteVector<T.Dialog>(WriteSerializable));
                Write(Messages, bw, WriteVector<T.Message>(WriteSerializable));
                Write(Chats, bw, WriteVector<T.Chat>(WriteSerializable));
                Write(Users, bw, WriteVector<T.User>(WriteSerializable));
                Write(State, bw, WriteSerializable);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var dialogs = Read(br, ReadVector(T.Dialog.Deserialize));
                var messages = Read(br, ReadVector(T.Message.Deserialize));
                var chats = Read(br, ReadVector(T.Chat.Deserialize));
                var users = Read(br, ReadVector(T.User.Deserialize));
                var state = Read(br, T.Updates.State.Deserialize);
                return new Tag(dialogs, messages, chats, users, state);
            }
        }

        readonly ITlTypeTag _tag;
        PeerDialogs(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator PeerDialogs(Tag tag) => new PeerDialogs(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static PeerDialogs Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (PeerDialogs) Tag.DeserializeTag(br);
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

        public bool Equals(PeerDialogs other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is PeerDialogs x && Equals(x);
        public static bool operator ==(PeerDialogs a, PeerDialogs b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(PeerDialogs a, PeerDialogs b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(PeerDialogs other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is PeerDialogs x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(PeerDialogs a, PeerDialogs b) => a.CompareTo(b) <= 0;
        public static bool operator <(PeerDialogs a, PeerDialogs b) => a.CompareTo(b) < 0;
        public static bool operator >(PeerDialogs a, PeerDialogs b) => a.CompareTo(b) > 0;
        public static bool operator >=(PeerDialogs a, PeerDialogs b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}