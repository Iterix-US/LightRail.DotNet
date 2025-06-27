using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.EventArguments;

namespace SeroGlint.DotNet.NamedPipes.Delegates
{
    /// <summary>
    /// Delegate for handling pipe response requests.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate Task PipeResponseRequestedHandler(object sender, PipeResponseRequestedEventArgs args);
}
