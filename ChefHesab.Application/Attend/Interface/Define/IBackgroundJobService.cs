using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Define
{
    internal interface IBackgroundJobService
    {
        string EnqueueViewGenerationJob<T>(Func<T> viewModelGenerator,string userId);
    }
}
