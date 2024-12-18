#define LOGx

namespace Gguc.Aoc.Y2024.Services;

using System;
using System.Collections.Generic;
using System.IO;
using Gguc.Aoc.Y2024.Memory;

public class ChronospatialComputer
{
    private readonly Day17Memory _memory;

    public ChronospatialComputer(Day17Memory memory)
    {
        _memory = memory;
    }

    public bool ProcessNext()
    {
        var operation = _memory.Input[_memory.Index];
        ProcessOperation(operation);

        return _memory.Index < _memory.Size;
    }

    private void ProcessOperation(long operation)
    {
        // operation.Dump("operation");

        switch (operation)
        {
            case 0: Adv(); break;
            case 1: Bxl(); break;
            case 2: Bst(); break;
            case 3: Jnz(); break;
            case 4: Bxc(); break;
            case 5: Out(); break;
            case 6: Bdv(); break;
            case 7: Cdv(); break;
        }
    }

    private void Adv()
    {
        var value1 = _memory.RegisterA;
        var value2 = NextComboOperand();
        var value3 = Math.Pow(2, value2);

        var result = (long)(value1 / value3);

        $"ADV: {value1}/{value3}({value2})=>{result}".Dump("adv");

        _memory.RegisterA = result;

        _memory.Index++;
    }

    private void Bxl()
    {
        var value1 = _memory.RegisterB;
        var value2 = NextOperand();

        var result = BitwiseXor(value1, value2);

        $"BXL: {value1}::{value2}=>{result}".Dump("bxl");

        _memory.RegisterB = result;

        _memory.Index++;
    }

    private void Bst()
    {
        var value2 = NextComboOperand();

        var result = value2 % 8;

        $"BST: {value2}=>{result}".Dump("bst");

        _memory.RegisterB = result;

        _memory.Index++;
    }

    private void Jnz()
    {
        var value1 = _memory.RegisterA;
        var value2 = NextOperand();

        if (value1 == 0)
        {
            _memory.Index++;
            return;
        }

        $"JNZ: {value1}=>{value2}".Dump("jnz");

        _memory.Index = (int)value2;
    }

    private void Bxc()
    {
        ++_memory.Index;

        var value1 = _memory.RegisterB;
        var value2 = _memory.RegisterC;

        var result = BitwiseXor(value1, value2);

        $"BXC: {value1}::{value2}=>{result}".Dump("bxc");

        _memory.RegisterB = result;

        _memory.Index++;
    }

    private void Out()
    {
        var value2 = NextComboOperand();

        var result = value2 % 8;

        $"OUT: {value2}=>{result}".Dump("out");

        _memory.Output.Add(result);

        _memory.Index++;
    }

    private void Bdv()
    {
        var value1 = _memory.RegisterA;
        var value2 = NextComboOperand();
        var value3 = Math.Pow(2, value2);

        var result = (long)(value1 / value3);

        $"BDV: {value1}/{value3}({value2})=>{result}".Dump("bdv");

        _memory.RegisterB = result;

        _memory.Index++;
    }

    private void Cdv()
    {
        var value1 = _memory.RegisterA;
        var value2 = NextComboOperand();
        var value3 = Math.Pow(2, value2);

        var result = (long)(value1 / value3);

        $"CDV: {value1}/{value3}({value2})=>{result}".Dump("cdv");

        _memory.RegisterC = result;

        _memory.Index++;
    }

    private long NextOperand()
    {
        var value = _memory.Input[++_memory.Index];

        $"next=>{value}".Dump("next");

        return value;
    }

    private long NextComboOperand()
    {
        var value = _memory.Input[++_memory.Index];

        $"combo=>{value}".Dump("combo");

        return value switch
        {
            4 => _memory.RegisterA,
            5 => _memory.RegisterB,
            6 => _memory.RegisterC,
            7 => throw new InvalidDataException("NextComboOperand is 7"),
            _ => value,
        };
    }

    private long BitwiseXor(long value1, long value2)
    {
        var result = value1 ^ value2;

        // var b1 = value1.ToBinaryString();
        // var b2 = value2.ToBinaryString();
        // var b3 = result.ToBinaryString();
        // $"{b1}::{b2}=>{b3}".Dump("bxl");

        return result;
    }
}