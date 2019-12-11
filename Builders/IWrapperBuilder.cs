using System.Text;

namespace WrapperGenerator
{
    interface IWrapperBuilder
    {
        public string Name { get; }
        public string BuildWrapper(string Name, Function[] Exports);

    }
}
