using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nasa.MarsRoboticRover.Application.Interfaces
{
    public interface IParser
    {
        List<ICommand> Parse(string commandInput);
    }
}
