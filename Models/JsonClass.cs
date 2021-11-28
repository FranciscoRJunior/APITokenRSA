using System;
using System.Collections.Generic;

namespace APITokenTest.Models
{
    public class JsonClass
    {
        public string Identification;
        public string Simulator;
        public int Punctuation;
        public DateTime Start;
        public DateTime End;
        public List<Faults> Faults;
    }
}

public class Faults
{
    public Faults() { }
    public string Fault;
}
