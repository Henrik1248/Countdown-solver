using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Countdown_Numbers_Solver
{
    public partial class Form1 : Form
    {
        List<TextBox> numberButtons;
        public Form1()
        {
            InitializeComponent();
            numberButtons = new List<TextBox>() { numberTextBox1, numberTextBox2, numberTextBox3, numberTextBox4, numberTextBox5, numberTextBox6 };
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            int target = -1;
            List<int> numbers = new List<int>();
            bool validNumbersAndTarget = true;
            try
            {
                target = Convert.ToInt32(targetTextBox.Text);
            }
            catch
            {
                validNumbersAndTarget = false;
            }
            foreach (TextBox number in numberButtons)
            {
                try
                {
                    numbers.Add(Convert.ToInt32(number.Text));
                }
                catch
                {
                    validNumbersAndTarget = false;
                    break;
                }
            }
            if (!validNumbersAndTarget)
            {
                return;
            }
            int bestAnswer = Calculation(numbers, target);
            answerLabel.Text = bestAnswer.ToString();
            calculationLabel.Text = Working(numbers, bestAnswer);
        }
        static string Working(List<int> numbers, int closestValue)
        {
            numbers.Sort();
            numbers.Reverse();
            if (numbers.Count == 1)
            {
                return $"= {numbers[0]}";
            }
            if (numbers.Contains(closestValue))
            {
                return $"= {closestValue}";
            }
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    List<int> numsToTest = new List<int>() { numbers[i] + numbers[j], numbers[i] * numbers[j], numbers[i] - numbers[j], numbers[j] - numbers[i] };
                    if (numbers[j] != 0)
                    {
                        if (numbers[i] % numbers[j] == 0)
                        {
                            numsToTest.Add(numbers[i] / numbers[j]);
                        }
                    }
                    if (numbers[i] != 0)
                    {
                        if (numbers[j] % numbers[i] == 0)
                        {
                            numsToTest.Add(numbers[j] / numbers[i]);
                        }
                    }
                    if (numsToTest.Contains(closestValue))
                    {
                        int a = numbers[i];
                        int b = numbers[j];
                        switch (numsToTest.IndexOf(closestValue))
                        {
                            case 0:
                                return $"{a} + {b} = {a + b}";
                            case 1:
                                return $"{a} * {b} = {a * b}";
                            case 2:
                                return $"{a} - {b} = {a - b}";
                            case 3:
                                return $"{b} - {a} = {b - a}";
                            case 4:
                                if (b != 0)
                                {
                                    if (a % b == 0)
                                    {
                                        return $"{a} / {b} = {a / b}";
                                    }
                                }
                                return $"{b} / {a} = {b / a}";
                        }
                    }
                    List<int> tempNums = new List<int>();
                    for (int k = 0; k < numbers.Count; k++)
                    {
                        if (k != i && k != j)
                        {
                            tempNums.Add(numbers[k]);
                        }
                    }
                    List<int> calculatedValues = new List<int>();
                    for (int k = 0; k < numsToTest.Count; k++)
                    {
                        tempNums.Add(numsToTest[k]);
                        calculatedValues.Add(Calculation(tempNums, closestValue)); // closestValue or target ???
                        tempNums.Remove(numsToTest[k]);
                    }
                    if (calculatedValues.Contains(closestValue))
                    {
                        int a = numbers[i];
                        int b = numbers[j];
                        int index = calculatedValues.IndexOf(closestValue);
                        tempNums.Add(numsToTest[index]);
                        switch (index)
                        {
                            case 0:
                                return $"{a} + {b} = {a + b}, \n" + Working(tempNums, closestValue);
                            case 1:
                                return $"{a} * {b} = {a * b}, \n" + Working(tempNums, closestValue);
                            case 2:
                                return $"{a} - {b} = {a - b}, \n" + Working(tempNums, closestValue);
                            case 3:
                                return $"{b} - {a} = {b - a}, \n" + Working(tempNums, closestValue);
                            case 4:
                                if (b != 0)
                                {
                                    if (a % b == 0)
                                    {
                                        return $"{a} / {b} = {a / b}, \n" + Working(tempNums, closestValue);
                                    }
                                }
                                return $"{b} / {a} = {b / a}, \n" + Working(tempNums, closestValue);
                        }
                    }
                }
            }
            return "";
        }
        static int Calculation(List<int> numbers, int target)
        {
            if (numbers.Count == 1)
            {
                return numbers[0];
            }
            int minDist = 1000000000;
            int value = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {

                    List<int> tempNums = new List<int>();
                    for (int k = 0; k < numbers.Count; k++)
                    {
                        if (k != i && k != j)
                        {
                            tempNums.Add(numbers[k]);
                        }
                    }
                    List<int> numsToAdd = new List<int>() { numbers[i] + numbers[j], numbers[i] * numbers[j], numbers[i] - numbers[j], numbers[j] - numbers[i] };
                    if (numbers[j] != 0)
                    {
                        if (numbers[i] % numbers[j] == 0)
                        {
                            numsToAdd.Add(numbers[i] / numbers[j]);
                        }
                    }
                    if (numbers[i] != 0)
                    {
                        if (numbers[j] % numbers[i] == 0)
                        {
                            numsToAdd.Add(numbers[j] / numbers[i]);
                        }
                    }
                    if (numsToAdd.Contains(target))
                    {
                        return target;
                    }
                    List<int> calculatedValues = new List<int>();
                    for (int k = 0; k < numsToAdd.Count; k++)
                    {
                        tempNums.Add(numsToAdd[k]);
                        calculatedValues.Add(Calculation(tempNums, target));
                        tempNums.Remove(numsToAdd[k]);
                    }
                    int index = IsClosest(calculatedValues, target);
                    if (Math.Abs(calculatedValues[index] - target) < minDist)
                    {
                        value = calculatedValues[index];
                        minDist = Math.Abs(calculatedValues[index] - target);
                    }
                }
            }
            return value;
        }
        static int IsClosest(List<int> nums, int target)
        {
            int index = 0;
            int minDist = 1000000;
            for (int i = 0; i < nums.Count; i++)
            {
                if (Math.Abs(nums[i] - target) < minDist)
                {
                    index = i;
                    minDist = Math.Abs(nums[i] - target);
                }
            }
            return index;
        }
    }
}
