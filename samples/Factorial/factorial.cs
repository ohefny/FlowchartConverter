using System;
using System.Collections.Generic;
using System.Text;
public class MyProgram
{
    public static void Main(String[] args)
    {
 
        { 

            int fact;
            int x;
            input(out x) ;
            fact = 1;
            for ( int i = 1 ; i< x+1 ; i+=1 ) 
            { 

                fact = fact * i;
            }

            Console.WriteLine("factorial is") ;
            Console.WriteLine(fact) ;
        }
        
    }
    
    // .NET can only read single characters or entire lines from the console.
    // The following functions are designed to help input specific data types.
    // 'out' is a pass-by-reference modifier in C#.
    
    private static void input(out double result)
    {
        while (!double.TryParse(Console.ReadLine(), out result));
    }
    
    private static void input(out int result)
    {
        while (!int.TryParse(Console.ReadLine(), out result));
    }
    
    private static void input(out bool result)
    {
        while (!bool.TryParse(Console.ReadLine(), out result));
    }
    
    private static void input(out string result)
    {
        result = Console.ReadLine();
    }
}

