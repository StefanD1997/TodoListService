using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Core.Common.Contracts
{
    public interface ICurrentUser
    {
        string Id { get; }
    }
}
