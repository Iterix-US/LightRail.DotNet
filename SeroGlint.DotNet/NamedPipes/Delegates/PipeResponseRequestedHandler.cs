using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.Delegates
{
    public delegate Task PipeResponseRequestedHandler(object sender, PipeResponseRequestedEventArgs args);
}
