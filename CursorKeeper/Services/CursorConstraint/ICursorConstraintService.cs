using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorKeeper.Services.CursorConstraint
{
    public interface ICursorConstraintService : IDisposable
    {
        bool IsEnabled { get; }
        void EnableConstraints();
        void DisableConstraints();
    }
}
