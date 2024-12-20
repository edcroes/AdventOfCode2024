namespace AoC2024.Day17;

public class Day17 : IMDay
{
    public string FilePath { private get; init; } = "Day17\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (computer, program) = await GetInput();

        computer.Execute(program);

        return string.Join(",", computer.Output);
    }

    /*
        My program looks like this:
        2,4 B = A % 8
        1,6 B = B ^ 6
        7,5 C = A / 2^B
        4,4 B = B ^ C
        1,7 B = B ^ 7
        0,3 A = A / 2^3
        5,5 Output B % 8
        3,0 Rerun of A != 0

        Some observations from this:
        - Putting A % 8 in B will result in B between 0 and 7 (3-bits)
        - B ^ 7 is the same as 7 - B
        - A / 2^3 is the same as A / 8
        - Output B % 8 outputs only the last 3 bits so the rest of B is not relevant

        Code for this looks like this:
        while (A != 0)
        {
            var B = A % 8;
            B ^= 6;
            C = A / Math.Pow(2, B);
            B ^= C;
            B = 7 - B;
            A /= 8;
            Output(B % 8);
        }

        This code will run until A is 0. Every loop will output a 3-bit number (0-7). It needs to copy the progam which is 16 numbers long.
        This means we need to loop 16 times.
        
        Let's dig into the code:
        var B = A % 8;           Takes the last 3 bits from A and put it in B
        var B ^= 6;              Flip the 3-bit number to another number, eg. 0 becomes 6 and 6 becomes 0 (0 <=> 6, 1 <=> 7, 2 <=> 4, 3 <=> 5)
        C = A / Math.Pow(2, B)   A is divided by 2^B where B is 0-7. Dividing is done by 1, 2, 4, 8, 16, 32, 64, 128. Because dividing is done by a power of 2 you can also state that we just bitshift, eg. A / 2^4 is the same as A >> 4
        B ^= C                   B is flipped into another 3-bit number by C
        B = 7 - B                Nothing special
        A /= 8                   Divide A by 8 which is the same as bitshift right by 3 which is the same as lose the last 3 bits
        Output(B % 8)            Output the last 3-bits of B so the rest is not relevant

        Every loop we cut off 3 bits of A. This means that if we want 16 loops for our output we need an A with 16 * 3 = 48 bits where the 3 most significant bits cannot all be 0.
        At the beginning of the loop we'll take the 3 least significant bits and work with that, at the end we'll get rid of those bits.
        In other words, A looks like some kind of stack with 3-bit numbers, eg. when A is 1667498 its binary representation is 000110010111000110101010.
        If we split that up in 3-bit numbers we get:
        000|110|010|111|000|110|101|010

        Get rid of the first 3 bits since those are 0 and do not add anything. The following 3-bit numbers remain:
        110|010|111|000|110|101|010
         6 | 2 | 7 | 0 | 6 | 5 | 2

        So the number 1667498 is actually a stack with the 2 on top and the 6 at the bottom.
        To prove that this is the case we can reconstruct the 1667498 by adding:
        2 * 8^0 =       2
        5 * 8^1 =      40
        6 * 8^2 =     384
        0 * 8^3 =       0
        7 * 8^4 =   28672
        2 * 8^5 =   65536
        6 * 8^6 = 1572864

        The next interesting part is the line C = A / Math.Pow(2, B). As stated before, we can just bitshift right here. If B is 0, bitshift with 0, B is 1 bitshift with 1 B etc.
        So what we put in C is the rest of the stack without the last B number of bits. That number is then used to XOR with B which then is XORed with 7 and the last 3 bits of this outcome will be the output.
        That means that only the last 3 bits are relevant. Looking at it that way we can safely say that a 3-bit number is put in C which can be constructed of bits from 2 numbers from the stack.
        Let's take our stack in bits:
        110|010|111|000|110|101|010

        If we bitshift it right with 1 and take the 3 least significant bits then that will be C. So this bits would be 101 which is 5:
        110|010|111|000|110|101|010
                              1 01X

        Bitshift right with the maximum of 7 will set C to 3 (011)
        110|010|111|000|110|101|010
                      0 11X XXX XXX

        Now let's take this knowledge and see how we can determine what the input stack should be.
        So let's reverse the code with the input that we desire:

        Output(B % 8)           => Gives us 2
        B = 7 - 5               => B should now be 5
        B ^= C                  => To get a result of 5 we have the following options: B=0 & C=5, B=1 & C=4, B=2 & C=7, B=3 & C=6, B=4 & C=1, B=5 & C=0, B=6 & C=3, B=7 & C=2
        C = A / Math.Pow(2, B)  => The most simple one, B=0 and C=5 gives us an A of 5 (5 / 2^0 = 5 / 1 = 5). An A of 5 gives us a B of 3 in the step B = (A % 8) ^ 6. So this one cannot be true.
                                => B=1 and C=4 uses 1 bitshift to the right. So we need to take the next value on the stack into account for this one. What we need on the stack is ??1|00? (C is 4 remember)
                                   For this to be true the top of the stack should be 001 of 000 or just plain 1 or 0. The number of the next value on the stack can be 001 (1), 011 (3), 101 (5) or 111 (7).
                                   If B=1 it means that 1 ^ 6 = 7. So the top of the stack should be 7 for B to become 1. Since 7 is not 0 or 1 this is not an option.
                                => B=2 and C=7 uses 2 bitshifts to the right. So again, the next value on the stack is also taken into account. We need ?11|1?? for the next and current value on the stack.
                                   So the top of the stack should be 100 (4), 101 (5), 110 (6) or 111 (7) and the next value on the stack has options 011 (3) and 111 (7).
                                   B=2 means A should be 4. A should be 3 or 7 for the correct C so this one can also not be true.
                                => B=3 and C=6 uses 3 bitshifts which means that only the next number on the stack is used for C. The next number on the stack should be 6 for this to be true. A = 3 ^ 6 = 5.
                                   So this can only be true of the stack looks like:...|110|101 and that makes this an option.
                                => B=4 and C=1 uses 4 bitshifts which is now using the next value on the stack and the value after that. So with C=1 it would look like this on the stack: ??0|01?|AAA.
                                   Next value on the stack can be 010 (2) or 011 (3) and the value after that can be 000 (0), 010 (2), 100 (4) and 110 (6). A is 4 ^ 6 = 2. This is an option with the following stack options (2 * 4 = 8)
                                   ...|000|010|010
                                   ...|010|010|010
                                   ...|100|010|010
                                   ...|110|010|010
                                   ...|000|011|010
                                   ...|010|011|010
                                   ...|100|011|010
                                   ...|110|011|010
                                   Which of these can still be true will should be determined with the next output that should be considered
                                => B=5 and C=0 uses 5 bitshifts which is also using the next value on the stack and the value after that. So with C=0 it would look like this: ?00|0??|AAA.
                                   Next value on the stack can be 000 (0), 001 (1), 010 (2), 011 (3) and the value after that 000 (0) or 100 (4) which yields to 8 possibilities of valid stacks where A = 5 ^ 6 = 3.
                                => B=6 and C=3 uses 6 bitshifts which means that only the value after the next value is considered which should be C.
                                   With A = 6 ^ 6 = 0 a valid option for his will look like this ...|011|???|000. The next value (???) can be anything.
                                => B=7 and C=2 uses 7 bitshifts so the value after the next value and the value after that are used for C. A = 7 ^ 6 = 1. We need a stack like ??0|10?|???|001. Again 8 options so one of those could be valid.
        B ^= 6                  => A = B ^ 6 (which can be multiple options looking at the previous statement)
        B = A % 8               => Put A on the bottom of the stack (if working from left to right)

        Especially the C = A / Math.Pow(2, B) sounds complicated but actually it's nothing more than bitshift the stack (as an integer) by B and AND it with 7 to get the last 3 bits: stack >> B & 7 = required C for this B.

        With the above findings we can have a lot of options for the first output to be correct. Looping through the next outputs we can eliminate options since a part of the stack is already known as an option.
        Since we need to look ahead in the stack, we get alot of options that also will be eliminated again in tests for the next outputs which is cumbersome.
        Let's make things easier and start iterating the output from end to start. In this way we're building the stack as we go without all those options. We only need to consider the options that are already set on the stack!
        Instead of writing that all down again, let's just program this...
    */
    public async Task<string> GetAnswerPart2()
    {
        var (_, program) = await GetInput();
        List<long> stackOptions = [0];
        var xor = program[3]; // Remember program is 2,4,1,6,... of which the 6 is the xor value

        foreach (var output in program.Reverse())
        {            
            List<long> newStackOptions = [];
            var bXorC = 7 - output;
            IEnumerable<(int a, int b, int c)> bcOptions = Enumerable.Range(0, 8).Select(i => (i ^ xor, i, i ^ bXorC));

            foreach (var stackOption in stackOptions)
            {
                var validOptions = bcOptions.Where(o => o.c == ((stackOption * 8 + o.a) >> o.b & 7));

                foreach (var (validA, _, _) in validOptions)
                {
                    newStackOptions.Add(stackOption * 8 + validA);
                }
            }

            stackOptions = newStackOptions;
        }

        return stackOptions.Min().ToString();
    }

    private async Task<(Computer, int[])> GetInput()
    {
        var input = await FileParser.ReadLinesAsString(FilePath);
        var a = input[0].Split(": ")[1].ParseToLong();
        var b = input[1].Split(": ")[1].ParseToInt();
        var c = input[2].Split(": ")[1].ParseToInt();
        var program = input[3].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].ToIntArray(",");

        return (new(a, b, c), program);
    }

    private class Computer
    {
        private readonly Dictionary<int, Action<int>> _opcodes = [];

        public Computer(long a, long b, long c)
        {
            A = a;
            B = b;
            C = c;

            _opcodes.Add(0, i => A /= (long)Math.Pow(2, GetComboOperand(i)));
            _opcodes.Add(1, i => B ^= i);
            _opcodes.Add(2, i => B = GetComboOperand(i) % 8);
            _opcodes.Add(3, i => InstructionPointer = A != 0 ? InstructionPointer = i - 2 : InstructionPointer);
            _opcodes.Add(4, _ => B ^= C);
            _opcodes.Add(5, i => Output.Add((int)(GetComboOperand(i) % 8)));
            _opcodes.Add(6, i => B = A / (long)Math.Pow(2, GetComboOperand(i)));
            _opcodes.Add(7, i => C = A / (long)Math.Pow(2, GetComboOperand(i)));
        }

        private int InstructionPointer { get; set; } = 0;
        private long A { get; set; }
        private long B { get; set; }
        private long C { get; set; }

        public List<int> Output { get; } = [];

        public void Execute(int[] program)
        {
            while (InstructionPointer >= 0 && InstructionPointer < program.Length - 1)
            {
                _opcodes[program[InstructionPointer]](program[InstructionPointer + 1]);
                InstructionPointer += 2;
            }
        }

        private long GetComboOperand(int operand) =>
            operand switch
            {
                < 4 => operand,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new NotSupportedException($"{operand} is not a valid combo operand")
            };
    }
}
