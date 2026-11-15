using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        Damage, Range, Knockback,            // carta 0: ataque
        Speed, Jump,                         // carta 1: player
        TowerReduction, TowerHeal, TowerMaxHealth // carta 2: torre
    }

    [Header("Referencias")]
    public PlayerMoveControls player;
    public Tower tower;

    [Header("Ataque")]
    public int damageStep = 1;
    public float rangeStep = 0.15f;
    public float rangeMax = 1.6f;
    public float knockbackStep = 1.5f;

    [Header("Player")]
    public float speedStep = 0.5f;
    public float jumpStep = 0.7f;
    public int maxLevelSpeedJump = 5;

    [Header("Torre")]
    public float damageReductionPerLevel = 0.9f; // cada nivel multiplica el daño recibido por 0.9
    public float healPercent = 0.4f;             // cura el 40% de la vida máxima
    public int maxHealthStep = 20;

    // Mejoras de cada carta
    private readonly UpgradeType[][] pools =
    {
        new[] { UpgradeType.Damage, UpgradeType.Range, UpgradeType.Knockback },
        new[] { UpgradeType.Speed, UpgradeType.Jump },
        new[] { UpgradeType.TowerReduction, UpgradeType.TowerHeal, UpgradeType.TowerMaxHealth }
    };

    private int[] levels;
    private UpgradeType[] current = new UpgradeType[3];

    private void Awake()
    {
        levels = new int[System.Enum.GetValues(typeof(UpgradeType)).Length];
    }

    // Sortea una mejora por carta y devuelve los textos
    public string[] RollOptions()
    {
        string[] texts = new string[3];
        for (int card = 0; card < 3; card++)
        {
            UpgradeType[] pool = pools[card];
            UpgradeType pick = pool[0];

            // Hasta 10 intentos para evitar mejoras sin efecto (al máximo o torre llena)
            for (int i = 0; i < 10; i++)
            {
                pick = pool[Random.Range(0, pool.Length)];
                if (IsAvailable(pick)) break;
            }

            current[card] = pick;
            texts[card] = GetText(pick);
        }
        return texts;
    }

    private bool IsAvailable(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Speed:
            case UpgradeType.Jump:
                return levels[(int)type] < maxLevelSpeedJump;
            case UpgradeType.Range:
                return player != null && player.attackRadius < rangeMax;
            case UpgradeType.TowerHeal:
                return tower != null && !tower.IsFullHealth;
            default:
                return true;
        }
    }

    private string GetText(UpgradeType type)
    {
        int next = levels[(int)type] + 1;
        switch (type)
        {
            case UpgradeType.Damage:         return "+" + damageStep + " daño\n(nivel " + next + ")";
            case UpgradeType.Range:          return "Más alcance\n(nivel " + next + ")";
            case UpgradeType.Knockback:      return "Más empuje\n(nivel " + next + ")";
            case UpgradeType.Speed:          return "Más velocidad\n(nivel " + next + "/" + maxLevelSpeedJump + ")";
            case UpgradeType.Jump:           return "Más salto\n(nivel " + next + "/" + maxLevelSpeedJump + ")";
            case UpgradeType.TowerReduction: return "La torre recibe menos daño\n(nivel " + next + ")";
            case UpgradeType.TowerHeal:      return "Cura la torre un " + Mathf.RoundToInt(healPercent * 100) + "%";
            case UpgradeType.TowerMaxHealth: return "+" + maxHealthStep + " vida máx. torre\n(nivel " + next + ")";
        }
        return "";
    }

    // Se llama al pulsar una carta
    public void Apply(int card)
    {
        UpgradeType type = current[card];
        levels[(int)type]++;

        switch (type)
        {
            case UpgradeType.Damage:    player.daño += damageStep; break;
            case UpgradeType.Range:     player.attackRadius = Mathf.Min(player.attackRadius + rangeStep, rangeMax); break;
            case UpgradeType.Knockback: player.knockbackForce += knockbackStep; break;
            case UpgradeType.Speed:     player.speed += speedStep; break;
            case UpgradeType.Jump:      player.jumpForce += jumpStep; break;

            case UpgradeType.TowerReduction: tower.damageMultiplier *= damageReductionPerLevel; break;
            case UpgradeType.TowerHeal:      tower.Heal(Mathf.RoundToInt(tower.maxHealth * healPercent)); break;
            case UpgradeType.TowerMaxHealth: tower.IncreaseMaxHealth(maxHealthStep); break;
        }

        Debug.Log("Mejora aplicada: " + type + " (nivel " + levels[(int)type] + ")");
    }
}