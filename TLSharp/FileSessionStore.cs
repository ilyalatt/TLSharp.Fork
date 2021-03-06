using System.IO;
using System.Threading.Tasks;
using LanguageExt;
using TLSharp.Utils;
using static LanguageExt.Prelude;

namespace TLSharp
{
    public class FileSessionStore : ISessionStore
    {
        readonly string _fileName;
        readonly string _backupFileName;
        readonly TaskQueue _taskQueue = new TaskQueue();

        public FileSessionStore(Some<string> name)
        {
            _fileName = name;
            _backupFileName = _fileName + ".backup";
        }


        static async Task<byte[]> ReadFileBytes(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                var ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        static async Task<Option<Session>> Load(string fileName)
        {
            if (!File.Exists(fileName)) return None;

            var bts = await ReadFileBytes(fileName);
            return bts.Apply(BtHelpers.Deserialize(Session.Deserialize));
        }

        void RestoreBackup()
        {
            if (!File.Exists(_backupFileName)) return;
            if (File.Exists(_fileName)) File.Delete(_fileName);
            File.Move(_backupFileName, _fileName);
        }

        async Task<Option<Session>> LoadImpl()
        {
            RestoreBackup();
            return await Load(_fileName);
        }

        public Task<Option<Session>> Load() =>
            _taskQueue.Put(LoadImpl);


        static async Task WriteFileBytes(string fileName, byte[] bytes)
        {
            using (var fs = File.OpenWrite(fileName))
            {
                var ms = new MemoryStream(bytes);
                await ms.CopyToAsync(fs);
            }
        }


        void CreateBackup()
        {
            RestoreBackup();
            if (File.Exists(_fileName)) File.Move(_fileName, _backupFileName);
        }

        void DeleteBackup()
        {
            if (File.Exists(_fileName) && File.Exists(_backupFileName)) File.Delete(_backupFileName);
        }

        static async Task Save(string fileName, Session session)
        {
            var bts = BtHelpers.UsingMemBinWriter(session.Serialize);
            await WriteFileBytes(fileName, bts);
        }

        async Task SaveImpl(Session session)
        {
            CreateBackup();
            await Save(_fileName, session);
            DeleteBackup();
        }

        public async Task Save(Some<Session> someSession) =>
            await _taskQueue.Put(() => SaveImpl(someSession));
    }
}
