using osu.Framework.IO.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IWBTM.Game
{
    public class LevelAudioStore : IResourceStore<byte[]>
    {
        public byte[] Get(string name)
        {
            using (Stream sr = GetStream(name))
            {
                byte[] buffer = new byte[sr.Length];
                sr.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public Task<byte[]> GetAsync(string name, CancellationToken cancellationToken = default) => null;

        public Stream GetStream(string name) => File.OpenRead($"Levels/{name}/audio.mp3");

        public IEnumerable<string> GetAvailableResources() => Enumerable.Empty<string>();

        #region IDisposable Support

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        ~LevelAudioStore()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
