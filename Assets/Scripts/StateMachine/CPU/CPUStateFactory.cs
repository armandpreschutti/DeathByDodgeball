using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUStateFactory
{
    CPUBrain _context;

    public CPUStateFactory(CPUBrain currentContext)
    {
        _context = currentContext;
    }

    public CPUBaseState Active()
    {
        return new CPUActiveState(_context, this);
    }
}


