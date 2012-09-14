#region
using System.Text;

#endregion

namespace VeeSharpTemplate
{
    public abstract class Script
    {
        private readonly StringBuilder _stringBuilder;

        protected Script() { _stringBuilder = new StringBuilder(); }

        public virtual void Run() { }

        // ReSharper disable UnusedMember.Global
        public void Write(string mString) { _stringBuilder.Append(mString); }
        public void WriteLine(string mString)
        {
            _stringBuilder.Append(mString);
            _stringBuilder.Append("\n");
        }
        // ReSharper restore UnusedMember.Global
        public string GetResult() { return _stringBuilder.ToString(); }
    }
}