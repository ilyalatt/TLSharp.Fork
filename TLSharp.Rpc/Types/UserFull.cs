using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class UserFull : ITlType, IEquatable<UserFull>, IComparable<UserFull>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            uint ITlTypeTag.TypeNumber => 0x0f220f3f;
            
            public bool Blocked { get; }
            public bool PhoneCallsAvailable { get; }
            public bool PhoneCallsPrivate { get; }
            public T.User User { get; }
            public Option<string> About { get; }
            public T.Contacts.Link Link { get; }
            public Option<T.Photo> ProfilePhoto { get; }
            public T.PeerNotifySettings NotifySettings { get; }
            public Option<T.BotInfo> BotInfo { get; }
            public int CommonChatsCount { get; }
            
            public Tag(
                bool blocked,
                bool phoneCallsAvailable,
                bool phoneCallsPrivate,
                Some<T.User> user,
                Option<string> about,
                Some<T.Contacts.Link> link,
                Option<T.Photo> profilePhoto,
                Some<T.PeerNotifySettings> notifySettings,
                Option<T.BotInfo> botInfo,
                int commonChatsCount
            ) {
                Blocked = blocked;
                PhoneCallsAvailable = phoneCallsAvailable;
                PhoneCallsPrivate = phoneCallsPrivate;
                User = user;
                About = about;
                Link = link;
                ProfilePhoto = profilePhoto;
                NotifySettings = notifySettings;
                BotInfo = botInfo;
                CommonChatsCount = commonChatsCount;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(MaskBit(0, Blocked) | MaskBit(4, PhoneCallsAvailable) | MaskBit(5, PhoneCallsPrivate) | MaskBit(1, About) | MaskBit(2, ProfilePhoto) | MaskBit(3, BotInfo), bw, WriteInt);
                Write(User, bw, WriteSerializable);
                Write(About, bw, WriteOption<string>(WriteString));
                Write(Link, bw, WriteSerializable);
                Write(ProfilePhoto, bw, WriteOption<T.Photo>(WriteSerializable));
                Write(NotifySettings, bw, WriteSerializable);
                Write(BotInfo, bw, WriteOption<T.BotInfo>(WriteSerializable));
                Write(CommonChatsCount, bw, WriteInt);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var flags = Read(br, ReadInt);
                var blocked = Read(br, ReadOption(flags, 0));
                var phoneCallsAvailable = Read(br, ReadOption(flags, 4));
                var phoneCallsPrivate = Read(br, ReadOption(flags, 5));
                var user = Read(br, T.User.Deserialize);
                var about = Read(br, ReadOption(flags, 1, ReadString));
                var link = Read(br, T.Contacts.Link.Deserialize);
                var profilePhoto = Read(br, ReadOption(flags, 2, T.Photo.Deserialize));
                var notifySettings = Read(br, T.PeerNotifySettings.Deserialize);
                var botInfo = Read(br, ReadOption(flags, 3, T.BotInfo.Deserialize));
                var commonChatsCount = Read(br, ReadInt);
                return new Tag(blocked, phoneCallsAvailable, phoneCallsPrivate, user, about, link, profilePhoto, notifySettings, botInfo, commonChatsCount);
            }
        }

        readonly ITlTypeTag _tag;
        UserFull(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator UserFull(Tag tag) => new UserFull(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static UserFull Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case 0x0f220f3f: return (UserFull) Tag.DeserializeTag(br);
                default: throw TlTransportException.UnexpectedTypeNumber(actual: typeNumber, expected: new uint[] { 0x0f220f3f });
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

        public bool Equals(UserFull other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is UserFull x && Equals(x);
        public static bool operator ==(UserFull a, UserFull b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(UserFull a, UserFull b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(UserFull other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is UserFull x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(UserFull a, UserFull b) => a.CompareTo(b) <= 0;
        public static bool operator <(UserFull a, UserFull b) => a.CompareTo(b) < 0;
        public static bool operator >(UserFull a, UserFull b) => a.CompareTo(b) > 0;
        public static bool operator >=(UserFull a, UserFull b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}