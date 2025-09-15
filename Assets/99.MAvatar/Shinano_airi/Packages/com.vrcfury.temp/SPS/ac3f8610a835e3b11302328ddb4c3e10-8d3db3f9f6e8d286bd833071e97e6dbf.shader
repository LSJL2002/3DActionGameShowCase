Shader "Hidden/SPSPatched/ac3f8610a835e3b11302328ddb4c3e10-8d3db3f9f6e8d286bd833071e97e6dbf"
{
    Properties
    {
        //----------------------------------------------------------------------------------------------------------------------
        // Dummy
        _DummyProperty ("If you are seeing this, some script is broken.", Float) = 0
        _DummyProperty ("This also happens if something other than lilToon is broken.", Float) = 0
        _DummyProperty ("You need to check the error on the console and take appropriate action, such as reinstalling the relevant tool.", Float) = 0
        _DummyProperty (" ", Float) = 0
        _DummyProperty ("これが表示されている場合、なんらかのスクリプトが壊れています。", Float) = 0
        _DummyProperty ("これはlilToon以外のものが壊れている場合にも発生します。", Float) = 0
        _DummyProperty ("コンソールでエラーを確認し、該当するツールを入れ直すなどの対処を行う必要があります。", Float) = 0
        [Space(1000)]
        _DummyProperty ("", Float) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Base
        [lilToggle]     _Invisible                  ("sInvisible", Int) = 0
                        _AsUnlit                    ("sAsUnlit", Range(0, 1)) = 0
                        _Cutoff                     ("sCutoff", Range(-0.001,1.001)) = 0.5
                        _SubpassCutoff              ("sSubpassCutoff", Range(0,1)) = 0.5
        [lilToggle]     _FlipNormal                 ("sFlipBackfaceNormal", Int) = 0
        [lilToggle]     _ShiftBackfaceUV            ("sShiftBackfaceUV", Int) = 0
                        _BackfaceForceShadow        ("sBackfaceForceShadow", Range(0,1)) = 0
        [lilHDR]        _BackfaceColor              ("sColor", Color) = (0,0,0,0)
                        _VertexLightStrength        ("sVertexLightStrength", Range(0,1)) = 0
                        _LightMinLimit              ("sLightMinLimit", Range(0,1)) = 0.05
                        _LightMaxLimit              ("sLightMaxLimit", Range(0,10)) = 1
                        _BeforeExposureLimit        ("sBeforeExposureLimit", Float) = 10000
                        _MonochromeLighting         ("sMonochromeLighting", Range(0,1)) = 0
                        _AlphaBoostFA               ("sAlphaBoostFA", Range(1,100)) = 10
                        _lilDirectionalLightStrength ("sDirectionalLightStrength", Range(0,1)) = 1
        [lilVec3B]      _LightDirectionOverride     ("sLightDirectionOverrides", Vector) = (0.001,0.002,0.001,0)
                        _AAStrength                 ("sAAShading", Range(0, 1)) = 1
        [lilToggle]     _UseDither                  ("sDither", Int) = 0
        [NoScaleOffset] _DitherTex                  ("Dither", 2D) = "white" {}
                        _DitherMaxValue             ("Max Value", Float) = 255

        //----------------------------------------------------------------------------------------------------------------------
        // Main
        [lilHDR] [MainColor] _Color                 ("sColor", Color) = (1,1,1,1)
        [MainTexture]   _MainTex                    ("Texture", 2D) = "white" {}
        [lilUVAnim]     _MainTex_ScrollRotate       ("sScrollRotates", Vector) = (0,0,0,0)
        [lilHSVG]       _MainTexHSVG                ("sHSVGs", Vector) = (0,1,1,1)
                        _MainGradationStrength      ("Gradation Strength", Range(0, 1)) = 0
        [NoScaleOffset] _MainGradationTex           ("Gradation Map", 2D) = "white" {}
        [NoScaleOffset] _MainColorAdjustMask        ("Adjust Mask", 2D) = "white" {}

        //----------------------------------------------------------------------------------------------------------------------
        // Main2nd
        [lilToggleLeft] _UseMain2ndTex              ("sMainColor2nd", Int) = 0
        [lilHDR]        _Color2nd                   ("sColor", Color) = (1,1,1,1)
                        _Main2ndTex                 ("Texture", 2D) = "white" {}
        [lilAngle]      _Main2ndTexAngle            ("sAngle", Float) = 0
        [lilUVAnim]     _Main2ndTex_ScrollRotate    ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _Main2ndTex_UVMode          ("UV Mode|UV0|UV1|UV2|UV3|MatCap", Int) = 0
        [lilEnum]       _Main2ndTex_Cull            ("sCullModes", Int) = 0
        [lilDecalAnim]  _Main2ndTexDecalAnimation   ("sDecalAnimations", Vector) = (1,1,1,30)
        [lilDecalSub]   _Main2ndTexDecalSubParam    ("sDecalSubParams", Vector) = (1,1,0,1)
        [lilToggle]     _Main2ndTexIsDecal          ("sAsDecal", Int) = 0
        [lilToggle]     _Main2ndTexIsLeftOnly       ("Left Only", Int) = 0
        [lilToggle]     _Main2ndTexIsRightOnly      ("Right Only", Int) = 0
        [lilToggle]     _Main2ndTexShouldCopy       ("Copy", Int) = 0
        [lilToggle]     _Main2ndTexShouldFlipMirror ("Flip Mirror", Int) = 0
        [lilToggle]     _Main2ndTexShouldFlipCopy   ("Flip Copy", Int) = 0
        [lilToggle]     _Main2ndTexIsMSDF           ("sAsMSDF", Int) = 0
        [NoScaleOffset] _Main2ndBlendMask           ("Mask", 2D) = "white" {}
        [lilEnum]       _Main2ndTexBlendMode        ("sBlendModes", Int) = 0
        [lilEnum]       _Main2ndTexAlphaMode        ("sAlphaModes", Int) = 0
                        _Main2ndEnableLighting      ("sEnableLighting", Range(0, 1)) = 1
                        _Main2ndDissolveMask        ("Dissolve Mask", 2D) = "white" {}
                        _Main2ndDissolveNoiseMask   ("Dissolve Noise Mask", 2D) = "gray" {}
        [lilUVAnim]     _Main2ndDissolveNoiseMask_ScrollRotate ("Scroll", Vector) = (0,0,0,0)
                        _Main2ndDissolveNoiseStrength ("Dissolve Noise Strength", float) = 0.1
        [lilHDR]        _Main2ndDissolveColor       ("sColor", Color) = (1,1,1,1)
        [lilDissolve]   _Main2ndDissolveParams      ("sDissolveParams", Vector) = (0,0,0.5,0.1)
        [lilDissolveP]  _Main2ndDissolvePos         ("Dissolve Position", Vector) = (0,0,0,0)
        [lilFFFB]       _Main2ndDistanceFade        ("sDistanceFadeSettings", Vector) = (0.1,0.01,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // Main3rd
        [lilToggleLeft] _UseMain3rdTex              ("sMainColor3rd", Int) = 0
        [lilHDR]        _Color3rd                   ("sColor", Color) = (1,1,1,1)
                        _Main3rdTex                 ("Texture", 2D) = "white" {}
        [lilAngle]      _Main3rdTexAngle            ("sAngle", Float) = 0
        [lilUVAnim]     _Main3rdTex_ScrollRotate    ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _Main3rdTex_UVMode          ("UV Mode|UV0|UV1|UV2|UV3|MatCap", Int) = 0
        [lilEnum]       _Main3rdTex_Cull            ("sCullModes", Int) = 0
        [lilDecalAnim]  _Main3rdTexDecalAnimation   ("sDecalAnimations", Vector) = (1,1,1,30)
        [lilDecalSub]   _Main3rdTexDecalSubParam    ("sDecalSubParams", Vector) = (1,1,0,1)
        [lilToggle]     _Main3rdTexIsDecal          ("sAsDecal", Int) = 0
        [lilToggle]     _Main3rdTexIsLeftOnly       ("Left Only", Int) = 0
        [lilToggle]     _Main3rdTexIsRightOnly      ("Right Only", Int) = 0
        [lilToggle]     _Main3rdTexShouldCopy       ("Copy", Int) = 0
        [lilToggle]     _Main3rdTexShouldFlipMirror ("Flip Mirror", Int) = 0
        [lilToggle]     _Main3rdTexShouldFlipCopy   ("Flip Copy", Int) = 0
        [lilToggle]     _Main3rdTexIsMSDF           ("sAsMSDF", Int) = 0
        [NoScaleOffset] _Main3rdBlendMask           ("Mask", 2D) = "white" {}
        [lilEnum]       _Main3rdTexBlendMode        ("sBlendModes", Int) = 0
        [lilEnum]       _Main3rdTexAlphaMode        ("sAlphaModes", Int) = 0
                        _Main3rdEnableLighting      ("sEnableLighting", Range(0, 1)) = 1
                        _Main3rdDissolveMask        ("Dissolve Mask", 2D) = "white" {}
                        _Main3rdDissolveNoiseMask   ("Dissolve Noise Mask", 2D) = "gray" {}
        [lilUVAnim]     _Main3rdDissolveNoiseMask_ScrollRotate ("Scroll", Vector) = (0,0,0,0)
                        _Main3rdDissolveNoiseStrength ("Dissolve Noise Strength", float) = 0.1
        [lilHDR]        _Main3rdDissolveColor       ("sColor", Color) = (1,1,1,1)
        [lilDissolve]   _Main3rdDissolveParams      ("sDissolveParams", Vector) = (0,0,0.5,0.1)
        [lilDissolveP]  _Main3rdDissolvePos         ("Dissolve Position", Vector) = (0,0,0,0)
        [lilFFFB]       _Main3rdDistanceFade        ("sDistanceFadeSettings", Vector) = (0.1,0.01,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // Alpha Mask
        [lilEnumLabel]  _AlphaMaskMode              ("sAlphaMaskModes", Int) = 0
                        _AlphaMask                  ("AlphaMask", 2D) = "white" {}
                        _AlphaMaskScale             ("Scale", Float) = 1
                        _AlphaMaskValue             ("Offset", Float) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // NormalMap
        [lilToggleLeft] _UseBumpMap                 ("sNormalMap", Int) = 0
        [Normal]        _BumpMap                    ("Normal Map", 2D) = "bump" {}
                        _BumpScale                  ("Scale", Range(-10,10)) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // NormalMap 2nd
        [lilToggleLeft] _UseBump2ndMap              ("sNormalMap2nd", Int) = 0
        [Normal]        _Bump2ndMap                 ("Normal Map", 2D) = "bump" {}
        [lilEnum]       _Bump2ndMap_UVMode          ("UV Mode|UV0|UV1|UV2|UV3", Int) = 0
                        _Bump2ndScale               ("Scale", Range(-10,10)) = 1
        [NoScaleOffset] _Bump2ndScaleMask           ("Mask", 2D) = "white" {}

        //----------------------------------------------------------------------------------------------------------------------
        // Anisotropy
        [lilToggleLeft] _UseAnisotropy              ("sAnisotropy", Int) = 0
        [Normal]        _AnisotropyTangentMap       ("Tangent Map", 2D) = "bump" {}
                        _AnisotropyScale            ("Scale", Range(-1,1)) = 1
        [NoScaleOffset] _AnisotropyScaleMask        ("Scale Mask", 2D) = "white" {}
                        _AnisotropyTangentWidth     ("sTangentWidth", Range(0,10)) = 1
                        _AnisotropyBitangentWidth   ("sBitangentWidth", Range(0,10)) = 1
                        _AnisotropyShift            ("sOffset", Range(-10,10)) = 0
                        _AnisotropyShiftNoiseScale  ("sNoiseStrength", Range(-1,1)) = 0
                        _AnisotropySpecularStrength ("sStrength", Range(0,10)) = 1
                        _Anisotropy2ndTangentWidth  ("sTangentWidth", Range(0,10)) = 1
                        _Anisotropy2ndBitangentWidth ("sBitangentWidth", Range(0,10)) = 1
                        _Anisotropy2ndShift         ("sOffset", Range(-10,10)) = 0
                        _Anisotropy2ndShiftNoiseScale ("sNoiseStrength", Range(-1,1)) = 0
                        _Anisotropy2ndSpecularStrength ("sStrength", Range(0,10)) = 0
                        _AnisotropyShiftNoiseMask   ("sNoise", 2D) = "white" {}
        [lilToggle]     _Anisotropy2Reflection      ("sReflection", Int) = 0
        [lilToggle]     _Anisotropy2MatCap          ("sMatCap", Int) = 0
        [lilToggle]     _Anisotropy2MatCap2nd       ("sMatCap2nd", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Backlight
        [lilToggleLeft] _UseBacklight               ("sBacklight", Int) = 0
        [lilHDR]        _BacklightColor             ("sColor", Color) = (0.85,0.8,0.7,1.0)
        [NoScaleOffset] _BacklightColorTex          ("Texture", 2D) = "white" {}
                        _BacklightMainStrength      ("sMainColorPower", Range(0, 1)) = 0
                        _BacklightNormalStrength    ("sNormalStrength", Range(0, 1)) = 1.0
                        _BacklightBorder            ("Border", Range(0, 1)) = 0.35
                        _BacklightBlur              ("sBlur", Range(0, 1)) = 0.05
                        _BacklightDirectivity       ("sDirectivity", Float) = 5.0
                        _BacklightViewStrength      ("sViewDirectionStrength", Range(0, 1)) = 1
        [lilToggle]     _BacklightReceiveShadow     ("sReceiveShadow", Int) = 1
        [lilToggle]     _BacklightBackfaceMask      ("sBackfaceMask", Int) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // Shadow
        [lilToggleLeft] _UseShadow                  ("sShadow", Int) = 0
                        _ShadowStrength             ("sStrength", Range(0, 1)) = 1
        [NoScaleOffset] _ShadowStrengthMask         ("sStrength", 2D) = "white" {}
        [lilLOD]        _ShadowStrengthMaskLOD      ("LOD", Range(0, 1)) = 0
        [NoScaleOffset] _ShadowBorderMask           ("sBorder", 2D) = "white" {}
        [lilLOD]        _ShadowBorderMaskLOD        ("LOD", Range(0, 1)) = 0
        [NoScaleOffset] _ShadowBlurMask             ("sBlur", 2D) = "white" {}
        [lilLOD]        _ShadowBlurMaskLOD          ("LOD", Range(0, 1)) = 0
        [lilFFFF]       _ShadowAOShift              ("1st Scale|1st Offset|2nd Scale|2nd Offset", Vector) = (1,0,1,0)
        [lilFF]         _ShadowAOShift2             ("3rd Scale|3rd Offset", Vector) = (1,0,1,0)
        [lilToggle]     _ShadowPostAO               ("sIgnoreBorderProperties", Int) = 0
        [lilEnum]       _ShadowColorType            ("sShadowColorTypes", Int) = 0
                        _ShadowColor                ("Shadow Color", Color) = (0.82,0.76,0.85,1.0)
        [NoScaleOffset] _ShadowColorTex             ("Shadow Color", 2D) = "black" {}
                        _ShadowNormalStrength       ("sNormalStrength", Range(0, 1)) = 1.0
                        _ShadowBorder               ("sBorder", Range(0, 1)) = 0.5
                        _ShadowBlur                 ("sBlur", Range(0, 1)) = 0.1
                        _ShadowReceive              ("sReceiveShadow", Range(0, 1)) = 0
                        _Shadow2ndColor             ("2nd Color", Color) = (0.68,0.66,0.79,1)
        [NoScaleOffset] _Shadow2ndColorTex          ("2nd Color", 2D) = "black" {}
                        _Shadow2ndNormalStrength    ("sNormalStrength", Range(0, 1)) = 1.0
                        _Shadow2ndBorder            ("sBorder", Range(0, 1)) = 0.15
                        _Shadow2ndBlur              ("sBlur", Range(0, 1)) = 0.1
                        _Shadow2ndReceive           ("sReceiveShadow", Range(0, 1)) = 0
                        _Shadow3rdColor             ("3rd Color", Color) = (0,0,0,0)
        [NoScaleOffset] _Shadow3rdColorTex          ("3rd Color", 2D) = "black" {}
                        _Shadow3rdNormalStrength    ("sNormalStrength", Range(0, 1)) = 1.0
                        _Shadow3rdBorder            ("sBorder", Range(0, 1)) = 0.25
                        _Shadow3rdBlur              ("sBlur", Range(0, 1)) = 0.1
                        _Shadow3rdReceive           ("sReceiveShadow", Range(0, 1)) = 0
                        _ShadowBorderColor          ("sShadowBorderColor", Color) = (1,0.1,0,1)
                        _ShadowBorderRange          ("sShadowBorderRange", Range(0, 1)) = 0.08
                        _ShadowMainStrength         ("sContrast", Range(0, 1)) = 0
                        _ShadowEnvStrength          ("sShadowEnvStrength", Range(0, 1)) = 0
        [lilEnum]       _ShadowMaskType             ("sShadowMaskTypes", Int) = 0
                        _ShadowFlatBorder           ("sBorder", Range(-2, 2)) = 1
                        _ShadowFlatBlur             ("sBlur", Range(0.001, 2)) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // Rim Shade
        [lilToggleLeft] _UseRimShade                ("RimShade", Int) = 0
                        _RimShadeColor              ("sColor", Color) = (0.5,0.5,0.5,1.0)
        [NoScaleOffset] _RimShadeMask               ("Mask", 2D) = "white" {}
                        _RimShadeNormalStrength     ("sNormalStrength", Range(0, 1)) = 1.0
                        _RimShadeBorder             ("sBorder", Range(0, 1)) = 0.5
                        _RimShadeBlur               ("sBlur", Range(0, 1)) = 1.0
        [PowerSlider(3.0)]_RimShadeFresnelPower     ("sFresnelPower", Range(0.01, 50)) = 1.0

        //----------------------------------------------------------------------------------------------------------------------
        // Reflection
        [lilToggleLeft] _UseReflection              ("sReflection", Int) = 0
        // Smoothness
                        _Smoothness                 ("Smoothness", Range(0, 1)) = 1
        [NoScaleOffset] _SmoothnessTex              ("Smoothness", 2D) = "white" {}
        // Metallic
        [Gamma]         _Metallic                   ("Metallic", Range(0, 1)) = 0
        [NoScaleOffset] _MetallicGlossMap           ("Metallic", 2D) = "white" {}
        // Reflectance
        [Gamma]         _Reflectance                ("sReflectance", Range(0, 1)) = 0.04
        // Reflection
                        _GSAAStrength               ("GSAA", Range(0, 1)) = 0
        [lilToggle]     _ApplySpecular              ("Apply Specular", Int) = 1
        [lilToggle]     _ApplySpecularFA            ("sMultiLightSpecular", Int) = 1
        [lilToggle]     _SpecularToon               ("Specular Toon", Int) = 1
                        _SpecularNormalStrength     ("sNormalStrength", Range(0, 1)) = 1.0
                        _SpecularBorder             ("sBorder", Range(0, 1)) = 0.5
                        _SpecularBlur               ("sBlur", Range(0, 1)) = 0.0
        [lilToggle]     _ApplyReflection            ("sApplyReflection", Int) = 0
                        _ReflectionNormalStrength   ("sNormalStrength", Range(0, 1)) = 1.0
        [lilHDR]        _ReflectionColor            ("sColor", Color) = (1,1,1,1)
        [NoScaleOffset] _ReflectionColorTex         ("sColor", 2D) = "white" {}
        [lilToggle]     _ReflectionApplyTransparency ("sApplyTransparency", Int) = 1
        [NoScaleOffset] _ReflectionCubeTex          ("Cubemap Fallback", Cube) = "black" {}
        [lilHDR]        _ReflectionCubeColor        ("sColor", Color) = (0,0,0,1)
        [lilToggle]     _ReflectionCubeOverride     ("Override", Int) = 0
                        _ReflectionCubeEnableLighting ("sEnableLighting+ (Fallback)", Range(0, 1)) = 1
        [lilEnum]       _ReflectionBlendMode        ("sBlendModes", Int) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // MatCap
        [lilToggleLeft] _UseMatCap                  ("sMatCap", Int) = 0
        [lilHDR]        _MatCapColor                ("sColor", Color) = (1,1,1,1)
                        _MatCapTex                  ("Texture", 2D) = "white" {}
                        _MatCapMainStrength         ("sMainColorPower", Range(0, 1)) = 0
        [lilVec2R]      _MatCapBlendUV1             ("sBlendUV1", Vector) = (0,0,0,0)
        [lilToggle]     _MatCapZRotCancel           ("sMatCapZRotCancel", Int) = 1
        [lilToggle]     _MatCapPerspective          ("sFixPerspective", Int) = 1
                        _MatCapVRParallaxStrength   ("sVRParallaxStrength", Range(0, 1)) = 1
                        _MatCapBlend                ("Blend", Range(0, 1)) = 1
        [NoScaleOffset] _MatCapBlendMask            ("Mask", 2D) = "white" {}
                        _MatCapEnableLighting       ("sEnableLighting", Range(0, 1)) = 1
                        _MatCapShadowMask           ("sShadowMask", Range(0, 1)) = 0
        [lilToggle]     _MatCapBackfaceMask         ("sBackfaceMask", Int) = 0
                        _MatCapLod                  ("sBlur", Range(0, 10)) = 0
        [lilEnum]       _MatCapBlendMode            ("sBlendModes", Int) = 1
        [lilToggle]     _MatCapApplyTransparency    ("sApplyTransparency", Int) = 1
                        _MatCapNormalStrength       ("sNormalStrength", Range(0, 1)) = 1.0
        [lilToggle]     _MatCapCustomNormal         ("sMatCapCustomNormal", Int) = 0
        [Normal]        _MatCapBumpMap              ("Normal Map", 2D) = "bump" {}
                        _MatCapBumpScale            ("Scale", Range(-10,10)) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // MatCap 2nd
        [lilToggleLeft] _UseMatCap2nd               ("sMatCap2nd", Int) = 0
        [lilHDR]        _MatCap2ndColor             ("sColor", Color) = (1,1,1,1)
                        _MatCap2ndTex               ("Texture", 2D) = "white" {}
                        _MatCap2ndMainStrength      ("sMainColorPower", Range(0, 1)) = 0
        [lilVec2R]      _MatCap2ndBlendUV1          ("sBlendUV1", Vector) = (0,0,0,0)
        [lilToggle]     _MatCap2ndZRotCancel        ("sMatCapZRotCancel", Int) = 1
        [lilToggle]     _MatCap2ndPerspective       ("sFixPerspective", Int) = 1
                        _MatCap2ndVRParallaxStrength ("sVRParallaxStrength", Range(0, 1)) = 1
                        _MatCap2ndBlend             ("Blend", Range(0, 1)) = 1
        [NoScaleOffset] _MatCap2ndBlendMask         ("Mask", 2D) = "white" {}
                        _MatCap2ndEnableLighting    ("sEnableLighting", Range(0, 1)) = 1
                        _MatCap2ndShadowMask        ("sShadowMask", Range(0, 1)) = 0
        [lilToggle]     _MatCap2ndBackfaceMask      ("sBackfaceMask", Int) = 0
                        _MatCap2ndLod               ("sBlur", Range(0, 10)) = 0
        [lilEnum]       _MatCap2ndBlendMode         ("sBlendModes", Int) = 1
        [lilToggle]     _MatCap2ndApplyTransparency ("sApplyTransparency", Int) = 1
                        _MatCap2ndNormalStrength    ("sNormalStrength", Range(0, 1)) = 1.0
        [lilToggle]     _MatCap2ndCustomNormal      ("sMatCapCustomNormal", Int) = 0
        [Normal]        _MatCap2ndBumpMap           ("Normal Map", 2D) = "bump" {}
                        _MatCap2ndBumpScale         ("Scale", Range(-10,10)) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // Rim
        [lilToggleLeft] _UseRim                     ("sRimLight", Int) = 0
        [lilHDR]        _RimColor                   ("sColor", Color) = (0.66,0.5,0.48,1)
        [NoScaleOffset] _RimColorTex                ("Texture", 2D) = "white" {}
                        _RimMainStrength            ("sMainColorPower", Range(0, 1)) = 0
                        _RimNormalStrength          ("sNormalStrength", Range(0, 1)) = 1.0
                        _RimBorder                  ("sBorder", Range(0, 1)) = 0.5
                        _RimBlur                    ("sBlur", Range(0, 1)) = 0.65
        [PowerSlider(3.0)]_RimFresnelPower          ("sFresnelPower", Range(0.01, 50)) = 3.5
                        _RimEnableLighting          ("sEnableLighting", Range(0, 1)) = 1
                        _RimShadowMask              ("sShadowMask", Range(0, 1)) = 0.5
        [lilToggle]     _RimBackfaceMask            ("sBackfaceMask", Int) = 1
                        _RimVRParallaxStrength      ("sVRParallaxStrength", Range(0, 1)) = 1
        [lilToggle]     _RimApplyTransparency       ("sApplyTransparency", Int) = 1
                        _RimDirStrength             ("sRimLightDirection", Range(0, 1)) = 0
                        _RimDirRange                ("sRimDirectionRange", Range(-1, 1)) = 0
                        _RimIndirRange              ("sRimIndirectionRange", Range(-1, 1)) = 0
        [lilHDR]        _RimIndirColor              ("sColor", Color) = (1,1,1,1)
                        _RimIndirBorder             ("sBorder", Range(0, 1)) = 0.5
                        _RimIndirBlur               ("sBlur", Range(0, 1)) = 0.1
        [lilEnum]       _RimBlendMode               ("sBlendModes", Int) = 1

        //----------------------------------------------------------------------------------------------------------------------
        // Glitter
        [lilToggleLeft] _UseGlitter                 ("sGlitter", Int) = 0
        [lilEnum]       _GlitterUVMode              ("UV Mode|UV0|UV1", Int) = 0
        [lilHDR]        _GlitterColor               ("sColor", Color) = (1,1,1,1)
                        _GlitterColorTex            ("Texture", 2D) = "white" {}
        [lilEnum]       _GlitterColorTex_UVMode     ("UV Mode|UV0|UV1|UV2|UV3", Int) = 0
                        _GlitterMainStrength        ("sMainColorPower", Range(0, 1)) = 0
                        _GlitterNormalStrength      ("sNormalStrength", Range(0, 1)) = 1.0
                        _GlitterScaleRandomize      ("sRandomize+ (Size)", Range(0, 1)) = 0
        [lilToggle]     _GlitterApplyShape          ("Shape", Int) = 0
                        _GlitterShapeTex            ("Texture", 2D) = "white" {}
        [lilVec2]       _GlitterAtras               ("Atras", Vector) = (1,1,0,0)
        [lilToggle]     _GlitterAngleRandomize      ("sRandomize+ (+sAngle+)", Int) = 0
        [lilGlitParam1] _GlitterParams1             ("Tiling|Particle Size|Contrast", Vector) = (256,256,0.16,50)
        [lilGlitParam2] _GlitterParams2             ("sGlitterParams2", Vector) = (0.25,0,0,0)
                        _GlitterPostContrast        ("sPostContrast", Float) = 1
                        _GlitterSensitivity         ("Sensitivity", Float) = 0.25
                        _GlitterEnableLighting      ("sEnableLighting", Range(0, 1)) = 1
                        _GlitterShadowMask          ("sShadowMask", Range(0, 1)) = 0
        [lilToggle]     _GlitterBackfaceMask        ("sBackfaceMask", Int) = 0
        [lilToggle]     _GlitterApplyTransparency   ("sApplyTransparency", Int) = 1
                        _GlitterVRParallaxStrength  ("sVRParallaxStrength", Range(0, 1)) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Emmision
        [lilToggleLeft] _UseEmission                ("sEmission", Int) = 0
        [HDR][lilHDR]   _EmissionColor              ("sColor", Color) = (1,1,1,1)
                        _EmissionMap                ("Texture", 2D) = "white" {}
        [lilUVAnim]     _EmissionMap_ScrollRotate   ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _EmissionMap_UVMode         ("UV Mode|UV0|UV1|UV2|UV3|Rim", Int) = 0
                        _EmissionMainStrength       ("sMainColorPower", Range(0, 1)) = 0
                        _EmissionBlend              ("Blend", Range(0,1)) = 1
                        _EmissionBlendMask          ("Mask", 2D) = "white" {}
        [lilUVAnim]     _EmissionBlendMask_ScrollRotate ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _EmissionBlendMode          ("sBlendModes", Int) = 1
        [lilBlink]      _EmissionBlink              ("sBlinkSettings", Vector) = (0,0,3.141593,0)
        [lilToggle]     _EmissionUseGrad            ("sGradation", Int) = 0
        [NoScaleOffset] _EmissionGradTex            ("Gradation Texture", 2D) = "white" {}
                        _EmissionGradSpeed          ("Gradation Speed", Float) = 1
                        _EmissionParallaxDepth      ("sParallaxDepth", float) = 0
                        _EmissionFluorescence       ("sFluorescence", Range(0,1)) = 0
        // Gradation
        [HideInInspector] _egci ("", Int) = 2
        [HideInInspector] _egai ("", Int) = 2
        [HideInInspector] _egc0 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc1 ("", Color) = (1,1,1,1)
        [HideInInspector] _egc2 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc3 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc4 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc5 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc6 ("", Color) = (1,1,1,0)
        [HideInInspector] _egc7 ("", Color) = (1,1,1,0)
        [HideInInspector] _ega0 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega1 ("", Color) = (1,0,0,1)
        [HideInInspector] _ega2 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega3 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega4 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega5 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega6 ("", Color) = (1,0,0,0)
        [HideInInspector] _ega7 ("", Color) = (1,0,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // Emmision2nd
        [lilToggleLeft] _UseEmission2nd             ("sEmission2nd", Int) = 0
        [HDR][lilHDR]   _Emission2ndColor           ("sColor", Color) = (1,1,1,1)
                        _Emission2ndMap             ("Texture", 2D) = "white" {}
        [lilUVAnim]     _Emission2ndMap_ScrollRotate ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _Emission2ndMap_UVMode      ("UV Mode|UV0|UV1|UV2|UV3|Rim", Int) = 0
                        _Emission2ndMainStrength    ("sMainColorPower", Range(0, 1)) = 0
                        _Emission2ndBlend           ("Blend", Range(0,1)) = 1
                        _Emission2ndBlendMask       ("Mask", 2D) = "white" {}
        [lilUVAnim]     _Emission2ndBlendMask_ScrollRotate ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _Emission2ndBlendMode       ("sBlendModes", Int) = 1
        [lilBlink]      _Emission2ndBlink           ("sBlinkSettings", Vector) = (0,0,3.141593,0)
        [lilToggle]     _Emission2ndUseGrad         ("sGradation", Int) = 0
        [NoScaleOffset] _Emission2ndGradTex         ("Gradation Texture", 2D) = "white" {}
                        _Emission2ndGradSpeed       ("Gradation Speed", Float) = 1
                        _Emission2ndParallaxDepth   ("sParallaxDepth", float) = 0
                        _Emission2ndFluorescence    ("sFluorescence", Range(0,1)) = 0
        // Gradation
        [HideInInspector] _e2gci ("", Int) = 2
        [HideInInspector] _e2gai ("", Int) = 2
        [HideInInspector] _e2gc0 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc1 ("", Color) = (1,1,1,1)
        [HideInInspector] _e2gc2 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc3 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc4 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc5 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc6 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2gc7 ("", Color) = (1,1,1,0)
        [HideInInspector] _e2ga0 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga1 ("", Color) = (1,0,0,1)
        [HideInInspector] _e2ga2 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga3 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga4 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga5 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga6 ("", Color) = (1,0,0,0)
        [HideInInspector] _e2ga7 ("", Color) = (1,0,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // Parallax
        [lilToggleLeft] _UseParallax                ("sParallax", Int) = 0
        [lilToggle]     _UsePOM                     ("sPOM", Int) = 0
        [NoScaleOffset] _ParallaxMap                ("Parallax Map", 2D) = "gray" {}
                        _Parallax                   ("Parallax Scale", float) = 0.02
                        _ParallaxOffset             ("sParallaxOffset", float) = 0.5

        //----------------------------------------------------------------------------------------------------------------------
        // Distance Fade
        [lilHDR]        _DistanceFadeColor          ("sColor", Color) = (0,0,0,1)
        [lilFFFB]       _DistanceFade               ("sDistanceFadeSettings", Vector) = (0.1,0.01,0,0)
        [lilEnum]       _DistanceFadeMode           ("sDistanceFadeModes", Int) = 0
        [lilHDR]        _DistanceFadeRimColor       ("sColor", Color) = (0,0,0,0)
        [PowerSlider(3.0)]_DistanceFadeRimFresnelPower ("sFresnelPower", Range(0.01, 50)) = 5.0

        //----------------------------------------------------------------------------------------------------------------------
        // AudioLink
        [lilToggleLeft] _UseAudioLink               ("sAudioLink", Int) = 0
        [lilFRFR]       _AudioLinkDefaultValue      ("Strength|Blink Strength|Blink Speed|Blink Threshold", Vector) = (0.0,0.0,2.0,0.75)
        [lilEnum]       _AudioLinkUVMode            ("sAudioLinkUVModes", Int) = 1
        [lilALUVParams] _AudioLinkUVParams          ("Scale|Offset|sAngle|Band|Bass|Low Mid|High Mid|Treble", Vector) = (0.25,0,0,0.125)
        [lilVec3]       _AudioLinkStart             ("sAudioLinkStartPosition", Vector) = (0.0,0.0,0.0,0.0)
                        _AudioLinkMask              ("Mask", 2D) = "blue" {}
        [lilUVAnim]     _AudioLinkMask_ScrollRotate ("sScrollRotates", Vector) = (0,0,0,0)
        [lilEnum]       _AudioLinkMask_UVMode       ("UV Mode|UV0|UV1|UV2|UV3", Int) = 0
        [lilToggle]     _AudioLink2Main2nd          ("sMainColor2nd", Int) = 0
        [lilToggle]     _AudioLink2Main3rd          ("sMainColor3rd", Int) = 0
        [lilToggle]     _AudioLink2Emission         ("sEmission", Int) = 0
        [lilToggle]     _AudioLink2EmissionGrad     ("sEmission+sGradation", Int) = 0
        [lilToggle]     _AudioLink2Emission2nd      ("sEmission2nd", Int) = 0
        [lilToggle]     _AudioLink2Emission2ndGrad  ("sEmission2nd+sGradation", Int) = 0
        [lilToggle]     _AudioLink2Vertex           ("sVertex", Int) = 0
        [lilEnum]       _AudioLinkVertexUVMode      ("sAudioLinkVertexUVModes", Int) = 1
        [lilALUVParams] _AudioLinkVertexUVParams    ("Scale|Offset|sAngle|Band|Bass|Low Mid|High Mid|Treble", Vector) = (0.25,0,0,0.125)
        [lilVec3]       _AudioLinkVertexStart       ("sAudioLinkStartPosition", Vector) = (0.0,0.0,0.0,0.0)
        [lilVec3Float]  _AudioLinkVertexStrength    ("sAudioLinkVertexStrengths", Vector) = (0.0,0.0,0.0,1.0)
        [lilToggle]     _AudioLinkAsLocal           ("sAudioLinkAsLocal", Int) = 0
        [NoScaleOffset] _AudioLinkLocalMap          ("Local Map", 2D) = "black" {}
        [lilALLocal]    _AudioLinkLocalMapParams    ("sAudioLinkLocalMapParams", Vector) = (120,1,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // Dissolve
                        _DissolveMask               ("Dissolve Mask", 2D) = "white" {}
                        _DissolveNoiseMask          ("Dissolve Noise Mask", 2D) = "gray" {}
        [lilUVAnim]     _DissolveNoiseMask_ScrollRotate ("Scroll", Vector) = (0,0,0,0)
                        _DissolveNoiseStrength      ("Dissolve Noise Strength", float) = 0.1
        [lilHDR]        _DissolveColor              ("sColor", Color) = (1,1,1,1)
        [lilDissolve]   _DissolveParams             ("sDissolveParamsModes", Vector) = (0,0,0.5,0.1)
        [lilDissolveP]  _DissolvePos                ("Dissolve Position", Vector) = (0,0,0,0)

        //----------------------------------------------------------------------------------------------------------------------
        // ID Mask
        // _IDMaskCompile will enable compilation of IDMask-related systems. For compatibility, setting certain
        // parameters to non-zero values will also enable the IDMask feature, but this enable switch ensures that
        // animator-controlled IDMasked meshes will be compiled correctly. Note that this _only_ controls compilation,
        // and is ignored at runtime.
        [ToggleUI]      _IDMaskCompile              ("_IDMaskCompile", Int) = 0
        [lilEnum]       _IDMaskFrom                 ("_IDMaskFrom|0: UV0|1: UV1|2: UV2|3: UV3|4: UV4|5: UV5|6: UV6|7: UV7|8: VertexID", Int) = 8
        [ToggleUI]      _IDMask1                    ("_IDMask1", Int) = 0
        [ToggleUI]      _IDMask2                    ("_IDMask2", Int) = 0
        [ToggleUI]      _IDMask3                    ("_IDMask3", Int) = 0
        [ToggleUI]      _IDMask4                    ("_IDMask4", Int) = 0
        [ToggleUI]      _IDMask5                    ("_IDMask5", Int) = 0
        [ToggleUI]      _IDMask6                    ("_IDMask6", Int) = 0
        [ToggleUI]      _IDMask7                    ("_IDMask7", Int) = 0
        [ToggleUI]      _IDMask8                    ("_IDMask8", Int) = 0
        [ToggleUI]      _IDMaskIsBitmap             ("_IDMaskIsBitmap", Int) = 0
                        _IDMaskIndex1               ("_IDMaskIndex1", Int) = 0
                        _IDMaskIndex2               ("_IDMaskIndex2", Int) = 0
                        _IDMaskIndex3               ("_IDMaskIndex3", Int) = 0
                        _IDMaskIndex4               ("_IDMaskIndex4", Int) = 0
                        _IDMaskIndex5               ("_IDMaskIndex5", Int) = 0
                        _IDMaskIndex6               ("_IDMaskIndex6", Int) = 0
                        _IDMaskIndex7               ("_IDMaskIndex7", Int) = 0
                        _IDMaskIndex8               ("_IDMaskIndex8", Int) = 0

        [ToggleUI]      _IDMaskControlsDissolve     ("_IDMaskControlsDissolve", Int) = 0
        [ToggleUI]      _IDMaskPrior1               ("_IDMaskPrior1", Int) = 0
        [ToggleUI]      _IDMaskPrior2               ("_IDMaskPrior2", Int) = 0
        [ToggleUI]      _IDMaskPrior3               ("_IDMaskPrior3", Int) = 0
        [ToggleUI]      _IDMaskPrior4               ("_IDMaskPrior4", Int) = 0
        [ToggleUI]      _IDMaskPrior5               ("_IDMaskPrior5", Int) = 0
        [ToggleUI]      _IDMaskPrior6               ("_IDMaskPrior6", Int) = 0
        [ToggleUI]      _IDMaskPrior7               ("_IDMaskPrior7", Int) = 0
        [ToggleUI]      _IDMaskPrior8               ("_IDMaskPrior8", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // UDIM Discard
        [lilToggleLeft] _UDIMDiscardCompile         ("sUDIMDiscard", Int) = 0
        [lilEnum]       _UDIMDiscardUV              ("sUDIMDiscardUV|0: UV0|1: UV1|2: UV2|3: UV3", Int) = 0
        [lilEnum]       _UDIMDiscardMode            ("sUDIMDiscardMode|0: Vertex|1: Pixel (slower)", Int) = 0
        [lilToggle]     _UDIMDiscardRow3_3          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow3_2          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow3_1          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow3_0          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow2_3          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow2_2          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow2_1          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow2_0          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow1_3          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow1_2          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow1_1          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow1_0          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow0_3          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow0_2          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow0_1          ("", Int) = 0
        [lilToggle]     _UDIMDiscardRow0_0          ("", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Encryption
        [lilToggle]     _IgnoreEncryption           ("sIgnoreEncryption", Int) = 0
                        _Keys                       ("sKeys", Vector) = (0,0,0,0)
                        _BitKey0                    ("_BitKey0", Float) = 0
                        _BitKey1                    ("_BitKey1", Float) = 0
                        _BitKey2                    ("_BitKey2", Float) = 0
                        _BitKey3                    ("_BitKey3", Float) = 0
                        _BitKey4                    ("_BitKey4", Float) = 0
                        _BitKey5                    ("_BitKey5", Float) = 0
                        _BitKey6                    ("_BitKey6", Float) = 0
                        _BitKey7                    ("_BitKey7", Float) = 0
                        _BitKey8                    ("_BitKey8", Float) = 0
                        _BitKey9                    ("_BitKey9", Float) = 0
                        _BitKey10                   ("_BitKey10", Float) = 0
                        _BitKey11                   ("_BitKey11", Float) = 0
                        _BitKey12                   ("_BitKey12", Float) = 0
                        _BitKey13                   ("_BitKey13", Float) = 0
                        _BitKey14                   ("_BitKey14", Float) = 0
                        _BitKey15                   ("_BitKey15", Float) = 0
                        _BitKey16                   ("_BitKey16", Float) = 0
                        _BitKey17                   ("_BitKey17", Float) = 0
                        _BitKey18                   ("_BitKey18", Float) = 0
                        _BitKey19                   ("_BitKey19", Float) = 0
                        _BitKey20                   ("_BitKey20", Float) = 0
                        _BitKey21                   ("_BitKey21", Float) = 0
                        _BitKey22                   ("_BitKey22", Float) = 0
                        _BitKey23                   ("_BitKey23", Float) = 0
                        _BitKey24                   ("_BitKey24", Float) = 0
                        _BitKey25                   ("_BitKey25", Float) = 0
                        _BitKey26                   ("_BitKey26", Float) = 0
                        _BitKey27                   ("_BitKey27", Float) = 0
                        _BitKey28                   ("_BitKey28", Float) = 0
                        _BitKey29                   ("_BitKey29", Float) = 0
                        _BitKey30                   ("_BitKey30", Float) = 0
                        _BitKey31                   ("_BitKey31", Float) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Outline
        [lilHDR]        _OutlineColor               ("sColor", Color) = (0.6,0.56,0.73,1)
                        _OutlineTex                 ("Texture", 2D) = "white" {}
        [lilUVAnim]     _OutlineTex_ScrollRotate    ("sScrollRotates", Vector) = (0,0,0,0)
        [lilHSVG]       _OutlineTexHSVG             ("sHSVGs", Vector) = (0,1,1,1)
        [lilHDR]        _OutlineLitColor            ("sColor", Color) = (1.0,0.2,0,0)
        [lilToggle]     _OutlineLitApplyTex         ("sColorFromMain", Int) = 0
                        _OutlineLitScale            ("Scale", Float) = 10
                        _OutlineLitOffset           ("Offset", Float) = -8
        [lilToggle]     _OutlineLitShadowReceive    ("sReceiveShadow", Int) = 0
        [lilOLWidth]    _OutlineWidth               ("Width", Range(0,1)) = 0.08
        [NoScaleOffset] _OutlineWidthMask           ("Width", 2D) = "white" {}
                        _OutlineFixWidth            ("sFixWidth", Range(0,1)) = 0.5
        [lilEnum]       _OutlineVertexR2Width       ("sOutlineVertexColorUsages", Int) = 0
        [lilToggle]     _OutlineDeleteMesh          ("sDeleteMesh0", Int) = 0
        [NoScaleOffset][Normal] _OutlineVectorTex   ("Vector", 2D) = "bump" {}
        [lilEnum]       _OutlineVectorUVMode        ("UV Mode|UV0|UV1|UV2|UV3", Int) = 0
                        _OutlineVectorScale         ("Vector scale", Range(-10,10)) = 1
                        _OutlineEnableLighting      ("sEnableLighting", Range(0, 1)) = 1
                        _OutlineZBias               ("Z Bias", Float) = 0
        [lilToggle]     _OutlineDisableInVR         ("sDisableInVR", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Tessellation
                        _TessEdge                   ("sTessellationEdge", Range(1, 100)) = 10
                        _TessStrength               ("sStrength", Range(0, 1)) = 0.5
                        _TessShrink                 ("sTessellationShrink", Range(0, 1)) = 0.0
        [IntRange]      _TessFactorMax              ("sTessellationFactor", Range(1, 8)) = 3

        //----------------------------------------------------------------------------------------------------------------------
        // For Multi
        [lilToggleLeft] _UseOutline                 ("Use Outline", Int) = 0
        [lilEnum]       _TransparentMode            ("Rendering Mode|Opaque|Cutout|Transparent|Refraction|Fur|FurCutout|Gem", Int) = 0
        [lilToggle]     _UseClippingCanceller       ("sSettingClippingCanceller", Int) = 0
        [lilToggle]     _AsOverlay                  ("sAsOverlay", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Save (Unused)
        [HideInInspector]                               _BaseColor          ("sColor", Color) = (1,1,1,1)
        [HideInInspector]                               _BaseMap            ("Texture", 2D) = "white" {}
        [HideInInspector]                               _BaseColorMap       ("Texture", 2D) = "white" {}
        [HideInInspector]                               _lilToonVersion     ("Version", Int) = 44

        //----------------------------------------------------------------------------------------------------------------------
        // Advanced
        [lilEnum]                                       _Cull               ("Cull Mode|Off|Front|Back", Int) = 2
        [Enum(UnityEngine.Rendering.BlendMode)]         _SrcBlend           ("sSrcBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _DstBlend           ("sDstBlendRGB", Int) = 10
        [Enum(UnityEngine.Rendering.BlendMode)]         _SrcBlendAlpha      ("sSrcBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _DstBlendAlpha      ("sDstBlendAlpha", Int) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]           _BlendOp            ("sBlendOpRGB", Int) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]           _BlendOpAlpha       ("sBlendOpAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _SrcBlendFA         ("sSrcBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _DstBlendFA         ("sDstBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _SrcBlendAlphaFA    ("sSrcBlendAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _DstBlendAlphaFA    ("sDstBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendOp)]           _BlendOpFA          ("sBlendOpRGB", Int) = 4
        [Enum(UnityEngine.Rendering.BlendOp)]           _BlendOpAlphaFA     ("sBlendOpAlpha", Int) = 4
        [lilToggle]                                     _ZClip              ("sZClip", Int) = 1
        [lilToggle]                                     _ZWrite             ("sZWrite", Int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]   _ZTest              ("sZTest", Int) = 4
        [IntRange]                                      _StencilRef         ("Ref", Range(0, 255)) = 0
        [IntRange]                                      _StencilReadMask    ("ReadMask", Range(0, 255)) = 255
        [IntRange]                                      _StencilWriteMask   ("WriteMask", Range(0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)]   _StencilComp        ("Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]         _StencilPass        ("Pass", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _StencilFail        ("Fail", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _StencilZFail       ("ZFail", Float) = 0
                                                        _OffsetFactor       ("sOffsetFactor", Float) = 0
                                                        _OffsetUnits        ("sOffsetUnits", Float) = 0
        [lilColorMask]                                  _ColorMask          ("sColorMask", Int) = 15
        [lilToggle]                                     _AlphaToMask        ("sAlphaToMask", Int) = 0
                                                        _lilShadowCasterBias ("Shadow Caster Bias", Float) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Outline Advanced
        [lilEnum]                                       _OutlineCull                ("Cull Mode|Off|Front|Back", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineSrcBlend            ("sSrcBlendRGB", Int) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineDstBlend            ("sDstBlendRGB", Int) = 10
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineSrcBlendAlpha       ("sSrcBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineDstBlendAlpha       ("sDstBlendAlpha", Int) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]           _OutlineBlendOp             ("sBlendOpRGB", Int) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]           _OutlineBlendOpAlpha        ("sBlendOpAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineSrcBlendFA          ("sSrcBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineDstBlendFA          ("sDstBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineSrcBlendAlphaFA     ("sSrcBlendAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _OutlineDstBlendAlphaFA     ("sDstBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendOp)]           _OutlineBlendOpFA           ("sBlendOpRGB", Int) = 4
        [Enum(UnityEngine.Rendering.BlendOp)]           _OutlineBlendOpAlphaFA      ("sBlendOpAlpha", Int) = 4
        [lilToggle]                                     _OutlineZClip               ("sZClip", Int) = 1
        [lilToggle]                                     _OutlineZWrite              ("sZWrite", Int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]   _OutlineZTest               ("sZTest", Int) = 2
        [IntRange]                                      _OutlineStencilRef          ("Ref", Range(0, 255)) = 0
        [IntRange]                                      _OutlineStencilReadMask     ("ReadMask", Range(0, 255)) = 255
        [IntRange]                                      _OutlineStencilWriteMask    ("WriteMask", Range(0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)]   _OutlineStencilComp         ("Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]         _OutlineStencilPass         ("Pass", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _OutlineStencilFail         ("Fail", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _OutlineStencilZFail        ("ZFail", Float) = 0
                                                        _OutlineOffsetFactor        ("sOffsetFactor", Float) = 0
                                                        _OutlineOffsetUnits         ("sOffsetUnits", Float) = 0
        [lilColorMask]                                  _OutlineColorMask           ("sColorMask", Int) = 15
        [lilToggle]                                     _OutlineAlphaToMask         ("sAlphaToMask", Int) = 0

        //----------------------------------------------------------------------------------------------------------------------
        // Pre
        [lilHDR] [MainColor]                            _PreColor               ("sColor", Color) = (1,1,1,1)
        [lilEnum]                                       _PreOutType             ("sPreOutTypes", Int) = 0
                                                        _PreCutoff              ("Pre Cutoff", Range(-0.001,1.001)) = 0.5
        [lilEnum]                                       _PreCull                ("Cull Mode|Off|Front|Back", Int) = 2
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreSrcBlend            ("sSrcBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreDstBlend            ("sDstBlendRGB", Int) = 10
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreSrcBlendAlpha       ("sSrcBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreDstBlendAlpha       ("sDstBlendAlpha", Int) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]           _PreBlendOp             ("sBlendOpRGB", Int) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]           _PreBlendOpAlpha        ("sBlendOpAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreSrcBlendFA          ("sSrcBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreDstBlendFA          ("sDstBlendRGB", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreSrcBlendAlphaFA     ("sSrcBlendAlpha", Int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]         _PreDstBlendAlphaFA     ("sDstBlendAlpha", Int) = 1
        [Enum(UnityEngine.Rendering.BlendOp)]           _PreBlendOpFA           ("sBlendOpRGB", Int) = 4
        [Enum(UnityEngine.Rendering.BlendOp)]           _PreBlendOpAlphaFA      ("sBlendOpAlpha", Int) = 4
        [lilToggle]                                     _PreZClip               ("sZClip", Int) = 1
        [lilToggle]                                     _PreZWrite              ("sZWrite", Int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]   _PreZTest               ("sZTest", Int) = 4
        [IntRange]                                      _PreStencilRef          ("Ref", Range(0, 255)) = 0
        [IntRange]                                      _PreStencilReadMask     ("ReadMask", Range(0, 255)) = 255
        [IntRange]                                      _PreStencilWriteMask    ("WriteMask", Range(0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)]   _PreStencilComp         ("Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]         _PreStencilPass         ("Pass", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _PreStencilFail         ("Fail", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]         _PreStencilZFail        ("ZFail", Float) = 0
                                                        _PreOffsetFactor        ("sOffsetFactor", Float) = 0
                                                        _PreOffsetUnits         ("sOffsetUnits", Float) = 0
        [lilColorMask]                                  _PreColorMask           ("sColorMask", Int) = 15
        [lilToggle]                                     _PreAlphaToMask         ("sAlphaToMask", Int) = 0
    }

    HLSLINCLUDE
        #define LIL_RENDER 2
    ENDHLSL

    SubShader
    {
        HLSLINCLUDE
            #define LIL_FEATURE_ANIMATE_MAIN_UV
            #define LIL_FEATURE_MAIN_TONE_CORRECTION
            #define LIL_FEATURE_MAIN_GRADATION_MAP
            #define LIL_FEATURE_MAIN2ND
            #define LIL_FEATURE_MAIN3RD
            #define LIL_FEATURE_DECAL
            #define LIL_FEATURE_ANIMATE_DECAL
            #define LIL_FEATURE_LAYER_DISSOLVE
            #define LIL_FEATURE_ALPHAMASK
            #define LIL_FEATURE_SHADOW
            #define LIL_FEATURE_RECEIVE_SHADOW
            #define LIL_FEATURE_SHADOW_3RD
            #define LIL_FEATURE_SHADOW_LUT
            #define LIL_FEATURE_RIMSHADE
            #define LIL_FEATURE_EMISSION_1ST
            #define LIL_FEATURE_EMISSION_2ND
            #define LIL_FEATURE_ANIMATE_EMISSION_UV
            #define LIL_FEATURE_ANIMATE_EMISSION_MASK_UV
            #define LIL_FEATURE_EMISSION_GRADATION
            #define LIL_FEATURE_NORMAL_1ST
            #define LIL_FEATURE_NORMAL_2ND
            #define LIL_FEATURE_ANISOTROPY
            #define LIL_FEATURE_REFLECTION
            #define LIL_FEATURE_MATCAP
            #define LIL_FEATURE_MATCAP_2ND
            #define LIL_FEATURE_RIMLIGHT
            #define LIL_FEATURE_RIMLIGHT_DIRECTION
            #define LIL_FEATURE_GLITTER
            #define LIL_FEATURE_BACKLIGHT
            #define LIL_FEATURE_PARALLAX
            #define LIL_FEATURE_POM
            #define LIL_FEATURE_DISTANCE_FADE
            #define LIL_FEATURE_AUDIOLINK
            #define LIL_FEATURE_AUDIOLINK_VERTEX
            #define LIL_FEATURE_AUDIOLINK_LOCAL
            #define LIL_FEATURE_DISSOLVE
            #define LIL_FEATURE_DITHER
            #define LIL_FEATURE_IDMASK
            #define LIL_FEATURE_UDIMDISCARD
            #define LIL_FEATURE_OUTLINE_TONE_CORRECTION
            #define LIL_FEATURE_OUTLINE_RECEIVE_SHADOW
            #define LIL_FEATURE_ANIMATE_OUTLINE_UV
            #define LIL_FEATURE_FUR_COLLISION
            #define LIL_FEATURE_MainGradationTex
            #define LIL_FEATURE_MainColorAdjustMask
            #define LIL_FEATURE_Main2ndTex
            #define LIL_FEATURE_Main2ndBlendMask
            #define LIL_FEATURE_Main2ndDissolveMask
            #define LIL_FEATURE_Main2ndDissolveNoiseMask
            #define LIL_FEATURE_Main3rdTex
            #define LIL_FEATURE_Main3rdBlendMask
            #define LIL_FEATURE_Main3rdDissolveMask
            #define LIL_FEATURE_Main3rdDissolveNoiseMask
            #define LIL_FEATURE_AlphaMask
            #define LIL_FEATURE_BumpMap
            #define LIL_FEATURE_Bump2ndMap
            #define LIL_FEATURE_Bump2ndScaleMask
            #define LIL_FEATURE_AnisotropyTangentMap
            #define LIL_FEATURE_AnisotropyScaleMask
            #define LIL_FEATURE_AnisotropyShiftNoiseMask
            #define LIL_FEATURE_ShadowBorderMask
            #define LIL_FEATURE_ShadowBlurMask
            #define LIL_FEATURE_ShadowStrengthMask
            #define LIL_FEATURE_ShadowColorTex
            #define LIL_FEATURE_Shadow2ndColorTex
            #define LIL_FEATURE_Shadow3rdColorTex
            #define LIL_FEATURE_RimShadeMask
            #define LIL_FEATURE_BacklightColorTex
            #define LIL_FEATURE_SmoothnessTex
            #define LIL_FEATURE_MetallicGlossMap
            #define LIL_FEATURE_ReflectionColorTex
            #define LIL_FEATURE_ReflectionCubeTex
            #define LIL_FEATURE_MatCapTex
            #define LIL_FEATURE_MatCapBlendMask
            #define LIL_FEATURE_MatCapBumpMap
            #define LIL_FEATURE_MatCap2ndTex
            #define LIL_FEATURE_MatCap2ndBlendMask
            #define LIL_FEATURE_MatCap2ndBumpMap
            #define LIL_FEATURE_RimColorTex
            #define LIL_FEATURE_GlitterColorTex
            #define LIL_FEATURE_GlitterShapeTex
            #define LIL_FEATURE_EmissionMap
            #define LIL_FEATURE_EmissionBlendMask
            #define LIL_FEATURE_EmissionGradTex
            #define LIL_FEATURE_Emission2ndMap
            #define LIL_FEATURE_Emission2ndBlendMask
            #define LIL_FEATURE_Emission2ndGradTex
            #define LIL_FEATURE_ParallaxMap
            #define LIL_FEATURE_AudioLinkMask
            #define LIL_FEATURE_AudioLinkLocalMap
            #define LIL_FEATURE_DissolveMask
            #define LIL_FEATURE_DissolveNoiseMask
            #define LIL_FEATURE_OutlineTex
            #define LIL_FEATURE_OutlineWidthMask
            #define LIL_FEATURE_OutlineVectorTex
            #define LIL_FEATURE_FurNoiseMask
            #define LIL_FEATURE_FurMask
            #define LIL_FEATURE_FurLengthMask
            #define LIL_FEATURE_FurVectorTex
            #define LIL_OPTIMIZE_APPLY_SHADOW_FA
            #define LIL_OPTIMIZE_USE_FORWARDADD
            #define LIL_OPTIMIZE_USE_VERTEXLIGHT
            #pragma skip_variants LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
            #pragma target 3.5
            #pragma fragmentoption ARB_precision_hint_fastest

            #pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma skip_variants _ADDITIONAL_LIGHT_SHADOWS
            #pragma skip_variants PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma skip_variants _SCREEN_SPACE_OCCLUSION
        ENDHLSL


        // Forward Back
        Pass
        {

            Name "FORWARD_BACK"
            Tags {"LightMode" = "ForwardBase"}

            Stencil
            {
                Ref [_PreStencilRef]
                ReadMask [_PreStencilReadMask]
                WriteMask [_PreStencilWriteMask]
                Comp [_PreStencilComp]
                Pass [_PreStencilPass]
                Fail [_PreStencilFail]
                ZFail [_PreStencilZFail]
            }
            Cull [_PreCull]
            ZClip [_PreZClip]
            ZWrite [_PreZWrite]
            ZTest [_PreZTest]
            ColorMask [_PreColorMask]
            Offset [_PreOffsetFactor], [_PreOffsetUnits]
            BlendOp [_PreBlendOp], [_PreBlendOpAlpha]
            Blend [_PreSrcBlend] [_PreDstBlend], [_PreSrcBlendAlpha] [_PreDstBlendAlpha]
            AlphaToMask [_PreAlphaToMask]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_vertex _ FOG_LINEAR FOG_EXP FOG_EXP2
            #pragma multi_compile_instancing
            #define LIL_PASS_FORWARD

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #define LIL_TRANSPARENT_PRE
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_forward.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        // Forward
        Pass
        {

            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}

            Stencil
            {
                Ref [_StencilRef]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilPass]
                Fail [_StencilFail]
                ZFail [_StencilZFail]
            }
            Cull [_Cull]
            ZClip [_ZClip]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            ColorMask [_ColorMask]
            Offset [_OffsetFactor], [_OffsetUnits]
            BlendOp [_BlendOp], [_BlendOpAlpha]
            Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
            AlphaToMask [_AlphaToMask]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_vertex _ FOG_LINEAR FOG_EXP FOG_EXP2
            #pragma multi_compile_instancing
            #define LIL_PASS_FORWARD

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_forward.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        // Forward Outline
        Pass
        {

            Name "FORWARD_OUTLINE"
            Tags {"LightMode" = "ForwardBase"}

            Stencil
            {
                Ref [_OutlineStencilRef]
                ReadMask [_OutlineStencilReadMask]
                WriteMask [_OutlineStencilWriteMask]
                Comp [_OutlineStencilComp]
                Pass [_OutlineStencilPass]
                Fail [_OutlineStencilFail]
                ZFail [_OutlineStencilZFail]
            }
            Cull [_OutlineCull]
            ZClip [_OutlineZClip]
            ZWrite [_OutlineZWrite]
            ZTest [_OutlineZTest]
            ColorMask [_OutlineColorMask]
            Offset [_OutlineOffsetFactor], [_OutlineOffsetUnits]
            BlendOp [_OutlineBlendOp], [_OutlineBlendOpAlpha]
            Blend [_OutlineSrcBlend] [_OutlineDstBlend], [_OutlineSrcBlendAlpha] [_OutlineDstBlendAlpha]
            AlphaToMask [_OutlineAlphaToMask]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_vertex _ FOG_LINEAR FOG_EXP FOG_EXP2
            #pragma multi_compile_instancing
            #define LIL_PASS_FORWARD

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #define LIL_OUTLINE
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_forward.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        //----------------------------------------------------------------------------------------------------------------------
        // ForwardAdd Start
        //

        // ForwardAdd
        Pass
        {

            Name "FORWARD_ADD"
            Tags {"LightMode" = "ForwardAdd"}

            Stencil
            {
                Ref [_StencilRef]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilPass]
                Fail [_StencilFail]
                ZFail [_StencilZFail]
            }
            Cull [_Cull]
            ZClip [_ZClip]
            ZWrite Off
            ZTest LEqual
            ColorMask [_ColorMask]
            Offset [_OffsetFactor], [_OffsetUnits]
            Blend [_SrcBlendFA] [_DstBlendFA], Zero One
            BlendOp [_BlendOpFA], [_BlendOpAlphaFA]
            AlphaToMask [_AlphaToMask]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_fragment POINT DIRECTIONAL SPOT POINT_COOKIE DIRECTIONAL_COOKIE
            #pragma multi_compile_vertex _ FOG_LINEAR FOG_EXP FOG_EXP2
            #pragma multi_compile_instancing
            #define LIL_PASS_FORWARDADD

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_forward.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        // ForwardAdd Outline
        Pass
        {

            Name "FORWARD_ADD_OUTLINE"
            Tags {"LightMode" = "ForwardAdd"}

            Stencil
            {
                Ref [_OutlineStencilRef]
                ReadMask [_OutlineStencilReadMask]
                WriteMask [_OutlineStencilWriteMask]
                Comp [_OutlineStencilComp]
                Pass [_OutlineStencilPass]
                Fail [_OutlineStencilFail]
                ZFail [_OutlineStencilZFail]
            }
            Cull [_OutlineCull]
            ZClip [_OutlineZClip]
            ZWrite Off
            ZTest LEqual
            ColorMask [_OutlineColorMask]
            Offset [_OutlineOffsetFactor], [_OutlineOffsetUnits]
            Blend [_OutlineSrcBlendFA] [_OutlineDstBlendFA], Zero One
            BlendOp [_OutlineBlendOpFA], [_OutlineBlendOpAlphaFA]
            AlphaToMask [_OutlineAlphaToMask]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_fragment POINT DIRECTIONAL SPOT POINT_COOKIE DIRECTIONAL_COOKIE
            #pragma multi_compile_vertex _ FOG_LINEAR FOG_EXP FOG_EXP2
            #pragma multi_compile_instancing
            #define LIL_PASS_FORWARDADD

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #define LIL_OUTLINE
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_forward.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        //
        // ForwardAdd End

        // ShadowCaster
        Pass
        {

            Name "SHADOW_CASTER"
            Tags {"LightMode" = "ShadowCaster"}
            Offset 1, 1
            Cull [_Cull]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #define LIL_PASS_SHADOWCASTER

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_shadowcaster.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        // ShadowCaster Outline
        Pass
        {

            Name "SHADOW_CASTER_OUTLINE"
            Tags {"LightMode" = "ShadowCaster"}
            Offset 1, 1
            Cull [_Cull]

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #define LIL_PASS_SHADOWCASTER

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #define LIL_OUTLINE
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_shadowcaster.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

        // Meta
        Pass
        {

            Name "META"
            Tags {"LightMode" = "Meta"}
            Cull Off

            HLSLPROGRAM


            //----------------------------------------------------------------------------------------------------------------------
            // Build Option
            // #pragma vertex vert
#pragma vertex spsVert

            #pragma fragment frag
            #pragma shader_feature EDITOR_VISUALIZATION
            #define LIL_PASS_META

            //----------------------------------------------------------------------------------------------------------------------
            // Pass
            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pipeline_brp.hlsl"

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_common.hlsl"

            // Insert functions and includes that depend on Unity here

            #include "Packages/jp.lilxyzw.liltoon/Shader\Includes/lil_pass_meta.hlsl"
struct SpsInputs : appdata {
    #if defined(LIL_APP_POSITION)
#define SPS_STRUCT_POSITION_TYPE_float4
#define SPS_STRUCT_POSITION_NAME positionOS
    #endif
    #if defined(LIL_APP_TEXCOORD0)
#define SPS_STRUCT_TEXCOORD_TYPE_float2
#define SPS_STRUCT_TEXCOORD_NAME uv0
    #endif
    #if defined(LIL_APP_TEXCOORD1)
#define SPS_STRUCT_TEXCOORD1_TYPE_float2
#define SPS_STRUCT_TEXCOORD1_NAME uv1
    #endif
    #if defined(LIL_APP_TEXCOORD2)
#define SPS_STRUCT_TEXCOORD2_TYPE_float2
#define SPS_STRUCT_TEXCOORD2_NAME uv2
    #endif
    #if defined(LIL_APP_TEXCOORD3)
#define SPS_STRUCT_TEXCOORD3_TYPE_float2
#define SPS_STRUCT_TEXCOORD3_NAME uv3
    #endif
    #if defined(LIL_APP_TEXCOORD4)
#define SPS_STRUCT_TEXCOORD4_TYPE_float2
#define SPS_STRUCT_TEXCOORD4_NAME uv4
    #endif
    #if defined(LIL_APP_TEXCOORD5)
#define SPS_STRUCT_TEXCOORD5_TYPE_float2
#define SPS_STRUCT_TEXCOORD5_NAME uv5
    #endif
    #if defined(LIL_APP_TEXCOORD6)
#define SPS_STRUCT_TEXCOORD6_TYPE_float2
#define SPS_STRUCT_TEXCOORD6_NAME uv6
    #endif
    #if defined(LIL_APP_TEXCOORD7)
#define SPS_STRUCT_TEXCOORD7_TYPE_float2
#define SPS_STRUCT_TEXCOORD7_NAME uv7
    #endif
    #if defined(LIL_APP_COLOR)
#define SPS_STRUCT_COLOR_TYPE_float4
#define SPS_STRUCT_COLOR_NAME color
    #endif
    #if defined(LIL_APP_NORMAL)
#define SPS_STRUCT_NORMAL_TYPE_float3
#define SPS_STRUCT_NORMAL_NAME normalOS
    #endif
    #if defined(LIL_APP_TANGENT)
#define SPS_STRUCT_TANGENT_TYPE_float4
#define SPS_STRUCT_TANGENT_NAME tangentOS
    #endif
    #if defined(LIL_APP_VERTEXID)
#define SPS_STRUCT_SV_VertexID_TYPE_uint
#define SPS_STRUCT_SV_VertexID_NAME vertexID
    #endif
    #if defined(LIL_APP_PREVPOS)
#define SPS_STRUCT_TEXCOORD4_TYPE_float3
#define SPS_STRUCT_TEXCOORD4_NAME previousPositionOS
    #endif
    #if defined(LIL_APP_PREVEL)
#define SPS_STRUCT_TEXCOORD5_TYPE_float3
#define SPS_STRUCT_TEXCOORD5_NAME precomputedVelocity
    #endif

// Add parameters needed by SPS if missing from the existing struct
#ifndef SPS_STRUCT_POSITION_NAME
  float3 spsPosition : POSITION;
  #define SPS_STRUCT_POSITION_TYPE_float3
  #define SPS_STRUCT_POSITION_NAME spsPosition
#endif
#ifndef SPS_STRUCT_NORMAL_NAME
  float3 spsNormal : NORMAL;
  #define SPS_STRUCT_NORMAL_TYPE_float3
  #define SPS_STRUCT_NORMAL_NAME spsNormal
#endif
#ifndef SPS_STRUCT_TANGENT_NAME
  float4 spsTangent : TANGENT;
  #define SPS_STRUCT_TANGENT_TYPE_float4
  #define SPS_STRUCT_TANGENT_NAME spsTangent
#endif
#ifndef SPS_STRUCT_SV_VertexID_NAME
  uint spsVertexId : SV_VertexID;
  #define SPS_STRUCT_SV_VertexID_TYPE_uint
  #define SPS_STRUCT_SV_VertexID_NAME spsVertexId
#endif
#ifndef SPS_STRUCT_COLOR_NAME
  float4 spsColor : COLOR;
  #define SPS_STRUCT_COLOR_TYPE_float4
  #define SPS_STRUCT_COLOR_NAME spsColor
#endif
};


#ifndef SPS_UTILS
#define SPS_UTILS

#include "UnityShaderVariables.cginc"

float sps_map(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

float sps_saturated_map(float value, float min, float max) {
    return saturate(sps_map(value, min, max, 0, 1));
}

// normalize fails fatally and discards the vert if length == 0
float3 sps_normalize(float3 a) {
    return length(a) == 0 ? float3(0,0,1) : normalize(a);
}

#define sps_angle_between(a,b) acos(dot(sps_normalize(a),sps_normalize(b)))

float3 sps_nearest_normal(float3 forward, float3 approximate) {
    return sps_normalize(cross(forward, cross(approximate, forward)));
}

float3 sps_toLocal(float3 v) { return mul(unity_WorldToObject, float4(v, 1)).xyz; }
float3 sps_toWorld(float3 v) { return mul(unity_ObjectToWorld, float4(v, 1)).xyz; }
// https://forum.unity.com/threads/point-light-in-v-f-shader.499717/#post-9052987
float sps_attenToRange(float atten) { return 5.0 * (1.0 / sqrt(atten)); }

#endif




// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
float3 sps_bezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		minT * minT * minT * p0
		+ 3 * minT * minT * t * p1
		+ 3 * minT * t * t * p2
		+ t * t * t * p3;
}
float3 sps_bezierDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
{
	const float minT = 1-t;
	return
		3 * minT * minT * (p1 - p0)
		+ 6 * minT * t * (p2 - p1)
		+ 3 * t * t * (p3 - p2);
}

// https://gamedev.stackexchange.com/questions/105230/points-evenly-spaced-along-a-bezier-curve
// https://gamedev.stackexchange.com/questions/5373/moving-ships-between-two-planets-along-a-bezier-missing-some-equations-for-acce/5427#5427
// https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf
// https://gamedev.stackexchange.com/questions/137022/consistent-normals-at-any-angle-in-bezier-curve/
void sps_bezierSolve(float3 p0, float3 p1, float3 p2, float3 p3, float lookingForLength, out float curveLength, out float3 position, out float3 forward, out float3 up)
{
	#define SPS_BEZIER_SAMPLES 50
	float sampledT[SPS_BEZIER_SAMPLES];
	float sampledLength[SPS_BEZIER_SAMPLES];
	float3 sampledUp[SPS_BEZIER_SAMPLES];
	float totalLength = 0;
	float3 lastPoint = p0;
	sampledT[0] = 0;
	sampledLength[0] = 0;
	sampledUp[0] = float3(0,1,0);
	{
		for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
		{
			const float t = float(i) / (SPS_BEZIER_SAMPLES-1);
			const float3 currentPoint = sps_bezier(p0, p1, p2, p3, t);
			const float3 currentForward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
			const float3 lastUp = sampledUp[i-1];
			const float3 currentUp = sps_nearest_normal(currentForward, lastUp);

			sampledT[i] = t;
			totalLength += length(currentPoint - lastPoint);
			sampledLength[i] = totalLength;
			sampledUp[i] = currentUp;
			lastPoint = currentPoint;
		}
	}

	float adjustedT = 1;
	float3 approximateUp = sampledUp[SPS_BEZIER_SAMPLES - 1];
	for(int i = 1; i < SPS_BEZIER_SAMPLES; i++)
	{
		if (lookingForLength <= sampledLength[i])
		{
			const float fraction = sps_map(lookingForLength, sampledLength[i-1], sampledLength[i], 0, 1);
			adjustedT = lerp(sampledT[i-1], sampledT[i], fraction);
			approximateUp = lerp(sampledUp[i-1], sampledUp[i], fraction);
			break;
		}
	}

	curveLength = totalLength;
	const float t = saturate(adjustedT);
	position = sps_bezier(p0, p1, p2, p3, t);
	forward = sps_normalize(sps_bezierDerivative(p0, p1, p2, p3, t));
	up = sps_nearest_normal(forward, approximateUp);
}


#include "UnityShaderVariables.cginc"
#ifndef SPS_GLOBALS
#define SPS_GLOBALS

#define SPS_PI float(3.14159265359)

#define SPS_TYPE_INVALID 0
#define SPS_TYPE_HOLE 1
#define SPS_TYPE_RING_TWOWAY 2
#define SPS_TYPE_SPSPLUS 3
#define SPS_TYPE_RING_ONEWAY 4
#define SPS_TYPE_FRONT 5

#ifdef SHADER_TARGET_SURFACE_ANALYSIS
    #define SPS_TEX_DEFINE(name) float4 name##_TexelSize; sampler2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) tex2Dlod(name, float4(uint2(x,y) * name##_TexelSize.xy, 0, 0))
#else
    #define SPS_TEX_DEFINE(name) Texture2D name;
    #define SPS_TEX_RAW_FLOAT4_XY(name,x,y) name[uint2(x,y)]
#endif
float SpsInt4ToFloat(int4 data) { return asfloat((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]); }
#define SPS_TEX_RAW_FLOAT4(tex, offset) ((float4)(SPS_TEX_RAW_FLOAT4_XY(tex, (offset)%8192, (offset)/8192)))
#define SPS_TEX_RAW_INT4(tex, offset) ((int4)(SPS_TEX_RAW_FLOAT4(tex, offset) * 255))
#define SPS_TEX_FLOAT(tex, offset) SpsInt4ToFloat(SPS_TEX_RAW_INT4(tex, offset))
#define SPS_TEX_FLOAT3(tex, offset) float3(SPS_TEX_FLOAT(tex, offset), SPS_TEX_FLOAT(tex, (offset)+1), SPS_TEX_FLOAT(tex, (offset)+2))

float _SPS_Length;
float _SPS_BakedLength;
SPS_TEX_DEFINE(_SPS_Bake)
float _SPS_BlendshapeVertCount;
float _SPS_Blendshape0;
float _SPS_Blendshape1;
float _SPS_Blendshape2;
float _SPS_Blendshape3;
float _SPS_Blendshape4;
float _SPS_Blendshape5;
float _SPS_Blendshape6;
float _SPS_Blendshape7;
float _SPS_Blendshape8;
float _SPS_Blendshape9;
float _SPS_Blendshape10;
float _SPS_Blendshape11;
float _SPS_Blendshape12;
float _SPS_Blendshape13;
float _SPS_Blendshape14;
float _SPS_Blendshape15;
float _SPS_BlendshapeCount;

float _SPS_Enabled;
float _SPS_Overrun;
float _SPS_Target_LL_Lights;

#endif




#ifndef SPS_PLUS
#define SPS_PLUS

float _SPS_Plus_Enabled;
float _SPS_Plus_Ring;
float _SPS_Plus_Hole;

void sps_plus_search(
    out float distance,
    out int type
) {
    distance = 999;
    type = SPS_TYPE_INVALID;
    
    float maxVal = 0;
    if (_SPS_Plus_Ring > maxVal) { maxVal = _SPS_Plus_Ring; type = SPS_TYPE_RING_TWOWAY; }
    if (_SPS_Plus_Hole == maxVal) { type = SPS_TYPE_RING_ONEWAY; }
    if (_SPS_Plus_Hole > maxVal) { maxVal = _SPS_Plus_Hole; type = SPS_TYPE_HOLE; }

    if (maxVal > 0) {
        distance = (1 - maxVal) * 3;
    }
}

#endif




void sps_light_parse(float range, half4 color, out int type) {
	type = SPS_TYPE_INVALID;

	if (range >= 0.5 || (length(color.rgb) > 0 && color.a > 0)) {
		// Outside of SPS range, or visible light
		return;
	}

	const int secondDecimal = round((range % 0.1) * 100);

	if (_SPS_Plus_Enabled > 0.5) {
		if (secondDecimal == 1 || secondDecimal == 2) {
			const int fourthDecimal = round((range % 0.001) * 10000);
			if (fourthDecimal == 2) {
				type = SPS_TYPE_SPSPLUS;
				return;
			}
		}
	}

	if (secondDecimal == 1) type = SPS_TYPE_HOLE;
	if (secondDecimal == 2) type = SPS_TYPE_RING_TWOWAY;
	if (secondDecimal == 5) type = SPS_TYPE_FRONT;
}

// Find nearby socket lights
void sps_light_search(
	inout int ioType,
	inout float3 ioRootLocal,
	inout float3 ioRootNormal,
	inout float4 ioColor
) {
	// Collect useful info about all the nearby lights that unity tells us about
	// (usually the brightest 4)
	int lightType[4];
	float3 lightWorldPos[4];
	float3 lightLocalPos[4];
	{
		for(int i = 0; i < 4; i++) {
	 		const float range = sps_attenToRange(unity_4LightAtten0[i]);
			sps_light_parse(range, unity_LightColor[i], lightType[i]);
	 		lightWorldPos[i] = float3(unity_4LightPosX0[i], unity_4LightPosY0[i], unity_4LightPosZ0[i]);
	 		lightLocalPos[i] = sps_toLocal(lightWorldPos[i]);
	 	}
	}

	// Fill in SPS light info from contacts
	if (_SPS_Plus_Enabled > 0.5) {
		float spsPlusDistance;
		int spsPlusType;
		sps_plus_search(spsPlusDistance, spsPlusType);
		if (spsPlusType != SPS_TYPE_INVALID) {
			bool spsLightFound = false;
			int spsLightIndex = 0;
			float spsMinError = 0;
			for(int i = 0; i < 4; i++) {
				if (lightType[i] != SPS_TYPE_SPSPLUS) continue;
				const float3 myPos = lightLocalPos[i];
				const float3 otherPos = lightLocalPos[spsLightIndex];
				const float myError = abs(length(myPos) - spsPlusDistance);
				if (myError > 0.2) continue;
				const float otherError = spsMinError;

				bool imBetter = false;
				if (!spsLightFound) imBetter = true;
				else if (myError < 0.3 && myPos.z >= 0 && otherPos.z < 0) imBetter = true;
				else if (otherError < 0.3 && otherPos.z >= 0 && myPos.z < 0) imBetter = false;
				else if (myError < otherError) imBetter = true;

				if (imBetter) {
					spsLightFound = true;
					spsLightIndex = i;
					spsMinError = myError;
				}
			}
			if (spsLightFound) {
				lightType[spsLightIndex] = spsPlusType;
			}
		}
	}

	// Find nearest socket root
	int rootIndex = 0;
	bool rootFound = false;
	{
	 	float minDistance = -1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distance = length(lightLocalPos[i]);
	 		const int type = lightType[i];
	 		if (distance < minDistance || minDistance < 0) {
	 			if (type == SPS_TYPE_HOLE || type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
	 				rootFound = true;
	 				rootIndex = i;
	 				minDistance = distance;
	 			}
	 		}
	 	}
	}
	
	int frontIndex = 0;
	bool frontFound = false;
	if (rootFound) {
	 	// Find front (normal) light for socket root if available
	 	float minDistance = 0.1;
	 	for(int i = 0; i < 4; i++) {
	 		const float distFromRoot = length(lightWorldPos[i] - lightWorldPos[rootIndex]);
	 		if (lightType[i] == SPS_TYPE_FRONT && distFromRoot < minDistance) {
	 			frontFound = true;
	 			frontIndex = i;
	 			minDistance = distFromRoot;
	 		}
	 	}
	}

	// This can happen if the socket was misconfigured, or if it's on a first person head bone that's been shrunk down
	// Ignore the normal, since it'll be so close to the root that rounding error will cause problems
	if (frontFound && length(lightLocalPos[frontIndex] - lightLocalPos[rootIndex]) < 0.00005) {
		frontFound = false;
	}

	if (!rootFound) return;
	if (ioType != SPS_TYPE_INVALID && length(lightLocalPos[rootIndex]) >= length(ioRootLocal)) return;

	ioType = lightType[rootIndex];
	ioRootLocal = lightLocalPos[rootIndex];
	ioRootNormal = frontFound
		? lightLocalPos[frontIndex] - lightLocalPos[rootIndex]
		: -1 * lightLocalPos[rootIndex];
	ioRootNormal = sps_normalize(ioRootNormal);
}











void SpsApplyBlendshape(uint vertexId, inout float3 position, inout float3 normal, inout float3 tangent, float blendshapeValue, int blendshapeId)
{
    if (blendshapeId >= _SPS_BlendshapeCount) return;
    const int vertCount = (int)_SPS_BlendshapeVertCount;
    const uint bytesPerBlendshapeVertex = 9;
    const uint bytesPerBlendshape = vertCount * bytesPerBlendshapeVertex + 1;
    const uint blendshapeOffset = 1 + (vertCount * 10) + bytesPerBlendshape * blendshapeId;
    const uint vertexOffset = blendshapeOffset + 1 + (vertexId * bytesPerBlendshapeVertex);
    const float blendshapeValueAtBake = SPS_TEX_FLOAT(_SPS_Bake, blendshapeOffset);
    const float blendshapeValueNow = blendshapeValue;
    const float change = (blendshapeValueNow - blendshapeValueAtBake) * 0.01;
    position += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset) * change;
    normal += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 3) * change;
    tangent += SPS_TEX_FLOAT3(_SPS_Bake, vertexOffset + 6) * change;
}

void SpsGetBakedPosition(uint vertexId, out float3 position, out float3 normal, out float3 tangent, out float active) {
    const uint bakeIndex = 1 + vertexId * 10;
    position = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex);
    normal = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 3);
    tangent = SPS_TEX_FLOAT3(_SPS_Bake, bakeIndex + 6);
    active = SPS_TEX_FLOAT(_SPS_Bake, bakeIndex + 9);
    if (position.z < 0) active = 0;

    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape0, 0);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape1, 1);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape2, 2);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape3, 3);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape4, 4);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape5, 5);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape6, 6);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape7, 7);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape8, 8);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape9, 9);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape10, 10);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape11, 11);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape12, 12);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape13, 13);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape14, 14);
    SpsApplyBlendshape(vertexId, position, normal, tangent, _SPS_Blendshape15, 15);

    position *= (_SPS_Length / _SPS_BakedLength);
}






// SPS Penetration Shader
void sps_apply_real(inout float3 vertex, inout float3 normal, inout float4 tangent, uint vertexId, inout float4 color)
{
	const float worldLength = _SPS_Length;
	const float3 origVertex = vertex;
	const float3 origNormal = normal;
	const float3 origTangent = tangent.xyz;
	float3 bakedVertex;
	float3 bakedNormal;
	float3 bakedTangent;
	float active;
	SpsGetBakedPosition(vertexId, bakedVertex, bakedNormal, bakedTangent, active);

	if (active == 0) return;

	float3 rootPos;
	int type = SPS_TYPE_INVALID;
	float3 frontNormal;
	sps_light_search(type, rootPos, frontNormal, color);
	if (type == SPS_TYPE_INVALID) return;

	float orfDistance = length(rootPos);
	float exitAngle = sps_angle_between(rootPos, float3(0,0,1));
	float entranceAngle = SPS_PI - sps_angle_between(frontNormal, rootPos);

	// Flip backward bidirectional rings
	if (type == SPS_TYPE_RING_TWOWAY && entranceAngle > SPS_PI/2) {
		frontNormal *= -1;
		entranceAngle = SPS_PI - entranceAngle;
	}

	// Decide if we should cancel deformation due to extreme angles, long distance, etc
	float bezierLerp;
	float dumbLerp;
	float shrinkLerp = 0;
	{
		float applyLerp = 1;
		// Cancel if base angle is too sharp
		const float allowedExitAngle = 0.6;
		const float exitAngleTooSharp = exitAngle > SPS_PI*allowedExitAngle ? 1 : 0;
		applyLerp = min(applyLerp, 1-exitAngleTooSharp);

		// Cancel if the entrance angle is too sharp
		if (type != SPS_TYPE_RING_TWOWAY) {
			const float allowedEntranceAngle = 0.8;
			const float entranceAngleTooSharp = entranceAngle > SPS_PI*allowedEntranceAngle ? 1 : 0;
			applyLerp = min(applyLerp, 1-entranceAngleTooSharp);
		}

		if (type == SPS_TYPE_HOLE) {
			// Uncancel if hilted in a hole
			const float hiltedSphereRadius = 0.5;
			const float inSphere = orfDistance > worldLength*hiltedSphereRadius ? 0 : 1;
			//const float hilted = min(isBehind, inSphere);
			//shrinkLerp = hilted;
			const float hilted = inSphere;
			applyLerp = max(applyLerp, hilted);
		} else {
			// Cancel if ring is near or behind base
			const float isBehind = rootPos.z > 0 ? 0 : 1;
			applyLerp = min(applyLerp, 1-isBehind);
		}

		// Cancel if too far away
		const float tooFar = sps_saturated_map(orfDistance, worldLength*1.2, worldLength*1.6);
		applyLerp = min(applyLerp, 1-tooFar);

		applyLerp = applyLerp * saturate(_SPS_Enabled);

		dumbLerp = sps_saturated_map(applyLerp, 0, 0.2) * active;
		bezierLerp = sps_saturated_map(applyLerp, 0, 1);
		shrinkLerp = sps_saturated_map(applyLerp, 0.8, 1) * shrinkLerp;
	}

	rootPos *= (1-shrinkLerp);
	orfDistance *= (1-shrinkLerp);

	float3 bezierPos;
	float3 bezierForward;
	float3 bezierRight;
	float3 bezierUp;
	float curveLength;
	if (length(rootPos) == 0) {
		bezierPos = float3(0,0,0);
		bezierForward = float3(0,0,1);
		bezierRight = float3(1,0,0);
		bezierUp = float3(0,1,0);
		curveLength = 0;
	} else{
		const float3 p0 = float3(0,0,0);
		const float p1Dist = min(orfDistance, max(worldLength / 8, rootPos.z / 4));
		const float p1DistWithPullout = sps_map(bezierLerp, 0, 1, worldLength * 5, p1Dist);
		const float3 p1 = float3(0,0,p1DistWithPullout);
		const float3 p2 = rootPos + frontNormal * p1Dist;
		const float3 p3 = rootPos;
		sps_bezierSolve(p0, p1, p2, p3, bakedVertex.z, curveLength, bezierPos, bezierForward, bezierUp);
		bezierRight = sps_normalize(cross(bezierUp, bezierForward));
	}

	// Handle holes and rings
	float holeShrink = 1;
	if (type == SPS_TYPE_RING_TWOWAY || type == SPS_TYPE_RING_ONEWAY) {
		if (bakedVertex.z >= curveLength) {
			// Straighten if past socket
			bezierPos += (bakedVertex.z - curveLength) * bezierForward;
		}
	} else {
		const float holeRecessDistance = worldLength * 0.05;
		const float holeRecessDistance2 = worldLength * 0.1;
		holeShrink = sps_saturated_map(
			bakedVertex.z,
			curveLength + holeRecessDistance2,
			curveLength + holeRecessDistance
		);
		if(_SPS_Overrun > 0) {
			if (bakedVertex.z >= curveLength + holeRecessDistance2) {
				// If way past socket, condense to point
				bezierPos += holeRecessDistance2 * bezierForward;
			} else if (bakedVertex.z >= curveLength) {
				// Straighten if past socket
				bezierPos += (bakedVertex.z - curveLength) * bezierForward;
			}
		}
	}

	float3 deformedVertex = bezierPos + bezierRight * bakedVertex.x * holeShrink + bezierUp * bakedVertex.y * holeShrink;
	vertex = lerp(origVertex, deformedVertex, dumbLerp);
	if (length(bakedNormal) != 0) {
		float3 deformedNormal = bezierRight * bakedNormal.x + bezierUp * bakedNormal.y + bezierForward * bakedNormal.z;
		normal = lerp(origNormal, deformedNormal, dumbLerp);
	}
	if (length(bakedTangent) != 0) {
		float3 deformedTangent = bezierRight * bakedTangent.x + bezierUp * bakedTangent.y + bezierForward * bakedTangent.z;
		tangent.xyz = lerp(origTangent, deformedTangent, dumbLerp);
	}
}
void sps_apply(inout SpsInputs o) {

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		float4 tangent = float4(o.SPS_STRUCT_TANGENT_NAME,1);
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		float4 color = float4(o.SPS_STRUCT_COLOR_NAME,1);
	#endif
	
	// When VERTEXLIGHT_ON is missing, there are no lights nearby, and the 4light arrays will be full of junk
	// Temporarily disable this check since apparently it causes some passes to not apply SPS
	//#ifdef VERTEXLIGHT_ON
	sps_apply_real(
		o.SPS_STRUCT_POSITION_NAME.xyz,
		o.SPS_STRUCT_NORMAL_NAME,
		#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
			tangent,
		#else
			o.SPS_STRUCT_TANGENT_NAME,
		#endif
		o.SPS_STRUCT_SV_VertexID_NAME,
		#if defined(SPS_STRUCT_COLOR_TYPE_float3)
			color
		#else
			o.SPS_STRUCT_COLOR_NAME
		#endif
	);
	//#endif

	#if defined(SPS_STRUCT_TANGENT_TYPE_float3)
		o.SPS_STRUCT_TANGENT_NAME = tangent.xyz;
	#endif
	#if defined(SPS_STRUCT_COLOR_TYPE_float3)
		o.SPS_STRUCT_COLOR_NAME = color.xyz;
	#endif
}


LIL_V2F_TYPE spsVert(SpsInputs input) {
  sps_apply(input);
  return vert((appdata)input);
}



            ENDHLSL
        
}

    }
    Fallback "Unlit/Texture"
}

