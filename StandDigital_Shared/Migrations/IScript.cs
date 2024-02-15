using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandDigital_Shared.Migrations;

internal interface IScript
{
    public string Script { get; init; }
}
