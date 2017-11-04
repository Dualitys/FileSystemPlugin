﻿using Plugin.Filesystem.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.Filesystem
{
    public class FileInfo : NativeItemWrapper<System.IO.FileInfo>, IFileInfo
    {
        public FileInfo(System.IO.FileInfo nativeItem) : base(nativeItem)
        {
        }

        public string Name => NativeItem.Name;

        public string FullName => NativeItem.FullName;

        public async Task<IFileInfo> CopyToAsync(IDirectoryInfo destFolder, string destFileName, bool overwrite = true)
        {
            var nativeFolder = (destFolder as NativeItemWrapper<System.IO.DirectoryInfo>).NativeItem;
            var newPath = Path.Combine(destFolder.FullName, destFileName);
            var newFile = await Task.Run(() => NativeItem.CopyTo(newPath, overwrite));
            return new FileInfo(newFile);
        }

        public Task DeleteAsync()
        {
            return Task.Run(() => NativeItem.Delete());
        }

        public Task<DateTimeOffset> GetLastModifiedAsync()
        {
            return Task.Run(() =>
            {
                return new DateTimeOffset(NativeItem.LastAccessTimeUtc, TimeSpan.Zero);
            });    
        }

        public Task<ulong> GetLengthAsync()
        {
            return Task.Run(() => (ulong)NativeItem.Length);
        }

        public Task<IDirectoryInfo> GetParentAsync()
        {
            return Task.Run(() =>
            {
                return new DirectoryInfo(NativeItem.Directory) as IDirectoryInfo;
            });
        }

        public Task MoveToAsync(IDirectoryInfo destFolder, string destFileName, bool overwrite = true)
        {
            var nativeFolder = (destFolder as NativeItemWrapper<System.IO.DirectoryInfo>).NativeItem;
            var newPath = Path.Combine(destFolder.FullName, destFileName);
            return Task.Run(() => NativeItem.MoveTo(newPath));
        }

        public Task<Stream> OpenAsync(FileAccess access)
        {
            return Task.Run(() => NativeItem.Open(FileMode.OpenOrCreate, access) as Stream);
        }
    }
}