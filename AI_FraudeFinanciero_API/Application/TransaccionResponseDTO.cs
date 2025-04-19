namespace AI_FraudeFinanciero_API.Application;

public class TransaccionResponseDTO
{    
    public float Score { get; set; }

    public float Probability { get; set; }

    public bool IsSospechosa { get; set; }
}
