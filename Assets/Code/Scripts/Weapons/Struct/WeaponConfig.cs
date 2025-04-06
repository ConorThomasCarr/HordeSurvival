

public struct WeaponConfig
{
    public WeaponConfig(WeaponGeneralConfig _generalConfig, WeaponCoreConfig _coreConfig)
    {
        generalConfig = _generalConfig;
        coreConfig = _coreConfig;
    }
    
    private WeaponGeneralConfig generalConfig {get; set;} 
   
    private WeaponCoreConfig coreConfig {get; set;}
    
    public WeaponGeneralConfig GeneralConfig { get => generalConfig; set => generalConfig = value; }

    public WeaponCoreConfig CoreConfig { get => coreConfig; set => coreConfig = value; }
    
}

