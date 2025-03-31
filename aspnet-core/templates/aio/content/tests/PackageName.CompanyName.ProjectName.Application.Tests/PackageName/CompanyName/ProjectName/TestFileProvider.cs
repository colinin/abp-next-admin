using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace PackageName.CompanyName.ProjectName;

public class TestFileProvider : IFileProvider
{
    private readonly Dictionary<string, IFileInfo> _files;

    public TestFileProvider()
    {
        _files = new Dictionary<string, IFileInfo>();
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return new NotFoundDirectoryContents();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        if (_files.TryGetValue(subpath, out var fileInfo))
        {
            return fileInfo;
        }
        return new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }

    public void AddFile(string path, string contents)
    {
        _files[path] = new TestFileInfo(path, contents);
    }
}

public class TestFileInfo : IFileInfo
{
    private readonly string _contents;

    public TestFileInfo(string name, string contents)
    {
        Name = name;
        _contents = contents;
    }

    public bool Exists => true;
    public long Length => _contents.Length;
    public string PhysicalPath => null;
    public string Name { get; }
    public DateTimeOffset LastModified => DateTimeOffset.UtcNow;
    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_contents));
    }
}