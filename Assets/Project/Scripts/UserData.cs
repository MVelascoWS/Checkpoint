using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Checkpoint/User Data", order = 1)]
public class UserData : ScriptableObject
{
    [Header("User Authentication Data")]
    [SerializeField] private string email;

    [SerializeField] private string code;

    [Header("Additional Info (Optional)")]
    [SerializeField] private string timestamp;

    [SerializeField] private bool isVerified;

    // Propiedades públicas
    public string Email
    {
        get => email;
        set => email = value;
    }

    public string Code
    {
        get => code;
        set => code = value;
    }

    public string Timestamp
    {
        get => timestamp;
        set => timestamp = value;
    }

    public bool IsVerified
    {
        get => isVerified;
        set => isVerified = value;
    }

    /// <summary>
    /// Limpia todos los datos almacenados
    /// </summary>
    public void Clear()
    {
        email = string.Empty;
        code = string.Empty;
        timestamp = string.Empty;
        isVerified = false;
    }

    /// <summary>
    /// Establece los datos de autenticación
    /// </summary>
    public void SetAuthData(string userEmail, string verificationCode)
    {
        email = userEmail;
        code = verificationCode;
        timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        isVerified = false;
    }

    public void SetEmailData(string userEmail)
    {
        email = userEmail;
    }

    public void SetCodeData(string verificationCode)
    {
        code = verificationCode;
        timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        isVerified = false;
    }

    /// <summary>
    /// Verifica si los datos son válidos
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(code);
    }

    /// <summary>
    /// Marca el código como verificado
    /// </summary>
    public void MarkAsVerified()
    {
        isVerified = true;
    }
}
