using System;
using System.Collections.Generic;

namespace ZbigniewJson.Models
{
    public class SpeakerModel
    {
        public string Speaker;
        public Guid GUID;
        public Dictionary<string, List<VariantsModel>> Variants;
    }
}
