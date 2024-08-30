// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<int> FixMe(List<List<int>> myList)
{
    var newList = new List<int>();

    if (myList.Count % 2 == 0)
    {
        // imperative code
        foreach (List<int> item in myList)
        {
            foreach (int element in item)
            {
                newList.Add(element);

            }
        }
    }
    else
    {
        // functional code
        newList = myList.SelectMany(x => x).ToList();
    }

    // sorting hierarchy:
    // desc by x % 5
    // desc by x
    //Do not perform explicit validations to fulfill only the requirements of the test cases
    //return newList.OrderBy(x => x % 5 + x / (newList.Max() * 2)).OrderDescending().ToList();

    return newList.OrderBy(x => x % 5).ThenByDescending(x => x).Reverse().ToList();
    
}

var case1 = new List<List<int>> {
    new() { 3, 4 },
    new() { 2, 6 }
};
var case2 = new List<List<int>> { 
    new() { 3, 4 }, 
    new() { 12, 32, 89 }, 
    new() { 0 } 
};
var case3 = new List<List<int>> { 
    new() { 3, 4 }, 
    new() { 12, 32, 89 }, 
    new() { 0 }, 
    new() { -1 } 
};
var case4 = new List<List<int>> { 
    new() { 3, 4 }, 
    new() { 12, 100, 89 }, 
    new() { 0 }, 
    new() { }, 
    new() { 56 } 
};


// Argumentos                                   Resultado
// [ [3, 4], [2, 6] ]                           4, 3, 2, 6
Console.WriteLine(string.Join(", ", FixMe(case1)));

// Argumentos                                   Resultado
// [ [3, 4], [12, 32, 89], [0] ]                4, 89, 3, 12, 32, 0
Console.WriteLine(string.Join(", ", FixMe(case2)));

// Argumentos                                   Resultado
// [ [3, 4], [12, 32, 89], [0], [-1] ]          4, 89, 3, 12, 32, 0, -1
Console.WriteLine(string.Join(", ", FixMe(case3)));

// Argumentos                                   Resultado
// [ [3, 4], [12, 100, 89], [0], [], [56] ]     4, 89, 3, 12, 56, 100, 0
Console.WriteLine(string.Join(", ", FixMe(case4)));
