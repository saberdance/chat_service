using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tsubasa
{
    public enum PasswordLevel
    {
        NoConstrains,
        NumeralAndLetter,
        NumeralAndAllCase,
        NumeralAndAllCaseAndSyntax
    }

    public record Password
    {
        public string Content { get; set; }
        public PasswordLevel Level { get; set; } = PasswordLevel.NumeralAndLetter;
        public int RequiredLength { get; set; } = 8;
        public Password(string content, PasswordLevel level, int requiredLength) 
        {
            Content = content;
            Level =level;
            RequiredLength = requiredLength;
        }
        public void Deconstruct(out string content, out PasswordLevel level, out int requiredLength) 
                        => (content, level, requiredLength) = (Content, Level, RequiredLength);

        public const int MinimumLength = 6;
        public const int MaximumLength = 16;
    }

    public class AccountUtil
    { 
        public static bool ValidPhoneSyntax(string p) => Regex.IsMatch(p, @"^1[3456789]\d{9}$");

        public static bool ValidPasswordSyntax(Password p) => p switch
        {
            //需求密码长度不得超过16不得低于6
            (_, _, > Password.MaximumLength or < Password.MinimumLength) => false,
            //检查密码长度是否符合需求
            var (content, _, minLength) when content.Length < minLength => false,
            var (content,level,_) => CheckPasswordContent(content,level),
            null => false,
        }; 

        private static bool CheckPasswordContent(string content, PasswordLevel level) => level switch
        {
            PasswordLevel.NoConstrains => true,
            PasswordLevel.NumeralAndLetter => Regex.IsMatch(content, "(?=.*[0-9])(?=.*[a-zA-Z])"),
            PasswordLevel.NumeralAndAllCase => Regex.IsMatch(content, "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])"),
            PasswordLevel.NumeralAndAllCaseAndSyntax => Regex.IsMatch(content, "(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9])"),
            _=>false
        };
    }
}
