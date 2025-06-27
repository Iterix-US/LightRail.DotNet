using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.EventArguments;

namespace SeroGlint.DotNet.NamedPipes.Delegates
{
    public delegate Task PipeServerStateChangedHandler(object sender, PipeServerStateChangedEventArgs args);
}
