using System;
using System.Collections.Generic;
using System.Text;
public class MyProgram
{
    public static void Main(String[] args)
    {
 
        { 

            int N;
            int First;
            First = 0;
            int Second;
            Second = 1;
            int next;
            Console.WriteLine("Enter the number of terms of Fibonacci series you want") ;
            input(out N) ;
            for ( int i = 1 ; i< N ; i+=1 ) 
            { 

                if ( i <= 1 ) 
                { 

                    next = i;
                }
                else 
                { 

                    next = First + Second;
                    First = Second;
                    Second = next;
                }

                Console.WriteLine(next) ;
            }

            input(out next) ;
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

