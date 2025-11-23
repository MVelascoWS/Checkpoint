# Instrucciones de Implementación - Waypoint Login UI

## 📁 Estructura de Archivos

Coloca los archivos en tu proyecto de Unity de la siguiente manera:

```
Assets/
├── UI/
│   ├── Backgrounds/
│   │   └── background.png          # Tu imagen de fondo
│   ├── Icons/
│   │   └── error-icon.png          # (Opcional) Icono de error
│   └── Login/
│       ├── WaypointLoginUI.uxml    # Archivo UXML
│       └── WaypointLoginUI.uss     # Archivo de estilos USS
└── Scripts/
    ├── WaypointManager.cs          # Script creado anteriormente
    └── WaypointLoginUIController.cs # Controlador de UI

```

## 🎨 Configuración de la Imagen de Fondo

1. **Importa tu imagen de fondo** a `Assets/UI/Backgrounds/`

2. **Actualiza la ruta en el USS**:
   - Abre `WaypointLoginUI.uss`
   - Busca la línea:
   ```css
   background-image: url('project://database/Assets/UI/Backgrounds/background.png');
   ```
   - Ajusta la ruta según donde hayas colocado tu imagen

3. **Configuración de importación de la imagen**:
   - Selecciona tu imagen en Unity
   - En el Inspector:
     - Texture Type: `Sprite (2D and UI)`
     - Sprite Mode: `Single`
     - Wrap Mode: `Clamp`
     - Filter Mode: `Bilinear`
     - Apply

## 🎮 Configuración de la Escena

### Paso 1: Crear el GameObject de UI

1. En tu escena, crea un GameObject vacío: `Right Click > Create Empty`
2. Nómbralo `LoginUI`
3. Agrega el componente `UI Document`:
   - `Add Component > UI Toolkit > UI Document`

### Paso 2: Asignar el UXML

1. Selecciona el GameObject `LoginUI`
2. En el Inspector, en el componente `UI Document`:
   - Arrastra `WaypointLoginUI.uxml` al campo **Source Asset**
   - El Panel Settings puede quedar en `Default`

### Paso 3: Agregar el Script Controlador

1. Con `LoginUI` seleccionado:
   - `Add Component > Waypoint Login UI Controller`
2. En el Inspector:
   - **UI Document**: Se asignará automáticamente (o arrástralo si no)
   - **Waypoint Manager**: Arrastra el GameObject que tiene el `WaypointManager.cs`

### Paso 4: Configurar el WaypointManager

1. Crea otro GameObject: `Right Click > Create Empty`
2. Nómbralo `WaypointManager`
3. Agrega el script `WaypointManager.cs` (el que creamos anteriormente)
4. Configura los campos:
   - **Client ID**: Tu ID del Ronin Developer Console
   - **Deep Link Callback URL**: Tu deep link (ej: `waypoint://open`)
   - **Use Testnet**: ✓ (marca para Saigon Testnet)

## 🔧 Panel Settings (Opcional pero Recomendado)

Para mejor control de la UI:

1. `Right Click en Assets > Create > UI Toolkit > Panel Settings Asset`
2. Nómbralo `LoginPanelSettings`
3. Configura:
   - **Scale Mode**: `Scale With Screen Size`
   - **Reference Resolution**: 1920 x 1080 (o tu resolución target)
   - **Screen Match Mode**: `Match Width Or Height`
   - **Match**: 0.5 (balance entre ancho y alto)
4. Asigna este Panel Settings al `UI Document`

## 📱 Configuración para Mobile

### Android

Asegúrate de tener configurado:

1. **Build Settings**:
   - Platform: Android
   - Minimum API Level: 24 o superior

2. **Deep Link** en `AndroidManifest.xml`:
```xml
<intent-filter>
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.DEFAULT" />
    <category android:name="android.intent.category.BROWSABLE" />
    <data android:scheme="waypoint" android:host="open" />
</intent-filter>
```

### iOS

1. **URL Types** en Info.plist:
   - Agrega tu URL Scheme: `waypoint`

## 🎨 Personalización de Estilos

### Cambiar Colores

En `WaypointLoginUI.uss`, busca y modifica:

```css
/* Color principal de botones */
.primary-button {
    background-color: rgba(100, 150, 255, 0.9); /* Cambiar aquí */
}

/* Color del fondo semi-transparente */
.content-container {
    background-color: rgba(0, 0, 0, 0.7); /* Ajustar transparencia */
}
```

### Cambiar Fuentes

Si tienes fuentes personalizadas:

1. Importa tu fuente TrueType (.ttf) o OpenType (.otf)
2. En el USS, agrega:
```css
.screen-title {
    -unity-font: url('project://database/Assets/Fonts/TuFuente.ttf');
}
```

### Ajustar Tamaños

Para dispositivos móviles más pequeños, ajusta en la sección `@media`:

```css
@media (max-width: 600px) {
    .screen-title {
        font-size: 24px; /* Más pequeño */
    }
}
```

## 🔄 Flujo de Pantallas

1. **Inicio** → Usuario ve "Tap to Play"
2. **Email** → Usuario ingresa su correo
3. **Código** → Usuario ingresa código de 6 dígitos
4. **Waypoint Auth** → Se conecta con Ronin Waypoint
5. **Success** → Usuario entra al juego

Si hay error en cualquier paso → **Pantalla de Error**

## 🧪 Testing

### Código de Prueba

Por defecto, el código de verificación válido es: **123456**

Puedes cambiarlo en `WaypointLoginUIController.cs`, línea ~370:

```csharp
// Simulación: código correcto es "123456"
bool isValid = code == "123456";
```

### Pruebas Recomendadas

1. ✅ Probar con email inválido (debe mostrar error)
2. ✅ Probar con email vacío (debe mostrar error)
3. ✅ Probar código incompleto (debe pedir 6 dígitos)
4. ✅ Probar código correcto (123456)
5. ✅ Probar "Resend Code"
6. ✅ Probar botones "Back"

## 🐛 Troubleshooting

### La imagen de fondo no se ve

- Verifica la ruta en el USS
- Asegúrate que la imagen está importada como Sprite
- Revisa que el path comienza con `project://database/Assets/`

### Los campos de texto no responden

- Verifica que el `UI Document` está correctamente asignado
- Revisa en la consola si hay errores de referencia null
- Asegúrate que el EventSystem está activo en la escena

### Los estilos no se aplican

- Verifica que el archivo `.uss` está en la misma carpeta que el `.uxml`
- El USS se carga automáticamente si tiene el mismo nombre que el UXML

### El código de 6 dígitos no avanza automáticamente

- Esto es normal en algunas versiones de Unity
- El usuario puede usar Tab o hacer clic manual
- La verificación funciona con el botón "Verify"

## 🚀 Próximos Pasos

1. **Integra con tu Backend**:
   - Reemplaza la simulación de envío de código
   - Implementa verificación real de códigos
   - Conecta con tu sistema de autenticación

2. **Mejora el WaypointManager**:
   - Maneja callbacks de éxito/error
   - Guarda el estado de autenticación
   - Implementa funciones de wallet

3. **Añade Animaciones** (opcional):
   - Transiciones entre pantallas
   - Animación del loading spinner
   - Efectos de hover mejorados

## 📝 Notas Importantes

- **Seguridad**: Nunca guardes credenciales sensibles en el cliente
- **Backend**: Implementa verificación del lado del servidor
- **Testing**: Prueba en múltiples dispositivos y resoluciones
- **Ronin Console**: Asegúrate de registrar correctamente tu app

## 🔗 Referencias

- [Ronin Waypoint Docs](https://docs.skymavis.com/mavis/ronin-waypoint/)
- [UI Toolkit Manual](https://docs.unity3d.com/Manual/UIElements.html)
- [Ronin Developer Console](https://developers.roninchain.com/console/applications)
