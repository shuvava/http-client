using System;
using System.IO;


namespace HttpService
{
    public class HttpStream : Stream
    {
        private readonly IDisposable[] _deps;
        private readonly Stream _stream;


        public HttpStream(Stream stream, params IDisposable[] deps)
        {
            _stream = stream;
            _deps = deps ?? new IDisposable[0];
        }


        public override bool CanRead => _stream.CanRead;


        public override bool CanSeek => _stream.CanSeek;


        public override bool CanWrite => _stream.CanWrite;


        public override long Length => _stream.Length;


        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }


        protected override void Dispose(bool disposing)
        {
            foreach (var dep in _deps)
            {
                dep.Dispose();
            }

            _stream?.Dispose();
            base.Dispose(disposing);
        }


        public override void Flush()
        {
            _stream.Flush();
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }


        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }
    }
}
