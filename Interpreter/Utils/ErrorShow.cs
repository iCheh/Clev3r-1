using Interpreter.CommonData;
using Interpreter.DataTemplates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Utils
{
    internal class ErrorShow
    {
        internal ErrorShow()
        {
        }
        internal void ShowToConsole(List<Errore> errors)
        {
            Console.WriteLine("Errors: " + errors.Count.ToString());
            foreach(var err in errors)
            {
                Console.WriteLine(err.ToString());
            }
        }

        internal void ShowToConsole(List<string> errors)
        {
            Console.WriteLine("Errors: " + errors.Count.ToString());
            foreach (var err in errors)
            {
                Console.WriteLine(err.ToString());
            }
        }

        internal void ShowToConsole()
        {
            if (Data.Errors.Count <= 0)
            {
                return;
            }
            Console.WriteLine("Errors: " + Data.Errors.Count.ToString());
            foreach (var err in Data.Errors)
            {
                Console.WriteLine(err.ToString());
            }
        }

        internal List<string> GetListErrors(List<Errore> errors)
        {
            var outErr = new List<string>();

            outErr.Add("Errors: " + errors.Count.ToString());

            foreach (var err in errors)
            {
                outErr.Add(err.ToString());
            }

            return outErr;
        }

        internal void ShowOldErrorsToConsole(List<string> errors)
        {
            foreach (var err in errors)
            {
                Console.WriteLine(err);
            }
        }
    }
}
