using Godot;
using System;

public class Tokens 
{
    public TokenType type { get; private set; }
    public String value { get; private set; }

    public Tokens(TokenType type, String value)
    {
        this.type = type;
        this.value = value;
    }
}
public enum TokenType
{
    LABEL,
    COLON,
    VALUE,
    ENDLINE
}