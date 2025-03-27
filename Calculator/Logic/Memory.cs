using System;
using System.Collections.Generic;

namespace Calculator.Logic
{
    public class Memory
    {
        private List<double> memoryStack = new List<double>();
        private double memoryValue = 0;
        public void AddToMemory(double value)
        {
            memoryValue += value;
            memoryStack.Add(memoryValue);
        }
        public void SubtractFromMemory(double value)
        {
            memoryValue -= value;
            memoryStack.Add(memoryValue);
        }
        public void StoreMemory(double value)
        {
            memoryValue = value;
            memoryStack.Add(value);
        }
        public double RecallMemory()
        {
            return memoryValue;
        }
        public void ClearMemory()
        {
            memoryValue = 0;
            memoryStack.Clear();
        }
        public List<double> GetMemoryStack()
        {
            return new List<double>(memoryStack);
        }
    }
}
