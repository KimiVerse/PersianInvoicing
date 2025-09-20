Write-Host "🔧 Fixing MainWindow constructor issue..." -ForegroundColor Yellow

# Fix App.xaml - Remove StartupUri
$appXamlContent = @'
<Application x:Class="PersianInvoicing.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Themes/Colors.xaml"/>
                <ResourceDictionary Source="Themes/InvoiceHeaderTemplates.xaml"/>
            </ResourceDictionary.MergedDictionaries>

            <!-- Vazirmatn Font -->
            <FontFamily x:Key="VazirmatnFont">/Fonts/#Vazirmatn</FontFamily>

            <!-- Built-in Converters Only -->
            <BooleanToVisibilityConverter x:Key="BooleanToVisibilityConverter"/>

            <!-- Base Styles -->
            <Style TargetType="{x:Type Control}">
                <Setter Property="FontFamily" Value="{StaticResource VazirmatnFont}" />
                <Setter Property="Foreground" Value="{StaticResource PrimaryTextBrush}" />
            </Style>
            <Style TargetType="{x:Type TextBlock}">
                <Setter Property="FontFamily" Value="{StaticResource VazirmatnFont}" />
                <Setter Property="Foreground" Value="{StaticResource PrimaryTextBrush}" />
            </Style>
            <Style TargetType="{x:Type Window}">
                <Setter Property="Background" Value="{StaticResource BackgroundGradientBrush}"/>
            </Style>

            <!-- Button Style -->
            <Style TargetType="Button">
                <Setter Property="Background" Value="{StaticResource PrimaryAccentBrush}"/>
                <Setter Property="Foreground" Value="{StaticResource PrimaryTextBrush}"/>
                <Setter Property="BorderThickness" Value="0"/>
                <Setter Property="Padding" Value="16,10"/>
                <Setter Property="Cursor" Value="Hand"/>
                <Setter Property="FontWeight" Value="Medium"/>
                <Setter Property="Template">
                    <Setter.Value>
                        <ControlTemplate TargetType="Button">
                            <Border x:Name="border" Background="{TemplateBinding Background}" CornerRadius="8">
                                <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                            </Border>
                            <ControlTemplate.Triggers>
                                <Trigger Property="IsMouseOver" Value="True">
                                    <Setter TargetName="border" Property="Background" Value="{StaticResource HoverBrush}"/>
                                </Trigger>
                                <Trigger Property="IsPressed" Value="True">
                                    <Setter TargetName="border" Property="Opacity" Value="0.7"/>
                                </Trigger>
                            </ControlTemplate.Triggers>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Style>

            <!-- TextBox Style -->
            <Style TargetType="TextBox">
                <Setter Property="Background" Value="{StaticResource SecondaryBackgroundBrush}"/>
                <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
                <Setter Property="Foreground" Value="{StaticResource PrimaryTextBrush}"/>
                <Setter Property="BorderThickness" Value="2"/>
                <Setter Property="Padding" Value="12,8"/>
                <Setter Property="VerticalContentAlignment" Value="Center"/>
                <Setter Property="FontSize" Value="14"/>
                <Setter Property="Template">
                    <Setter.Value>
                        <ControlTemplate TargetType="TextBox">
                            <Border x:Name="border" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}" CornerRadius="8">
                                <ScrollViewer x:Name="PART_ContentHost" Focusable="false" HorizontalScrollBarVisibility="Hidden" VerticalScrollBarVisibility="Hidden"/>
                            </Border>
                            <ControlTemplate.Triggers>
                                <Trigger Property="IsFocused" Value="True">
                                    <Setter TargetName="border" Property="BorderBrush" Value="{StaticResource PrimaryAccentBrush}"/>
                                    <Setter TargetName="border" Property="Background" Value="{StaticResource TertiaryBackgroundBrush}"/>
                                </Trigger>
                            </ControlTemplate.Triggers>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Style>

            <!-- TabControl Style -->
            <Style TargetType="TabControl">
                <Setter Property="Background" Value="Transparent"/>
                <Setter Property="BorderThickness" Value="0"/>
            </Style>
            <Style TargetType="TabItem">
                <Setter Property="FontFamily" Value="{StaticResource VazirmatnFont}"/>
                <Setter Property="FontSize" Value="15"/>
                <Setter Property="FontWeight" Value="Medium"/>
                <Setter Property="Padding" Value="20,12"/>
                <Setter Property="Margin" Value="2,0"/>
                <Setter Property="Template">
                    <Setter.Value>
                        <ControlTemplate TargetType="TabItem">
                            <Border x:Name="Border" Background="{StaticResource SecondaryBackgroundBrush}" BorderBrush="{StaticResource BorderBrush}" BorderThickness="1,1,1,0" CornerRadius="12,12,0,0" Margin="2,0">
                                <ContentPresenter x:Name="ContentSite" VerticalAlignment="Center" HorizontalAlignment="Center" ContentSource="Header" Margin="{TemplateBinding Padding}"/>
                            </Border>
                            <ControlTemplate.Triggers>
                                <Trigger Property="IsSelected" Value="True">
                                    <Setter TargetName="Border" Property="Background" Value="{StaticResource PrimaryAccentBrush}"/>
                                    <Setter Property="Foreground" Value="{StaticResource PrimaryTextBrush}"/>
                                    <Setter TargetName="Border" Property="BorderBrush" Value="{StaticResource PrimaryAccentBrush}"/>
                                </Trigger>
                                <Trigger Property="IsMouseOver" Value="True">
                                    <Setter TargetName="Border" Property="Background" Value="{StaticResource TertiaryBackgroundBrush}"/>
                                </Trigger>
                            </ControlTemplate.Triggers>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Style>
        </ResourceDictionary>
    </Application.Resources>
</Application>
'@

Set-Content -Path "App.xaml" -Value $appXamlContent
Write-Host "✓ Fixed App.xaml" -ForegroundColor Green

# Rebuild
Write-Host "🔨 Rebuilding application..." -ForegroundColor Yellow
dotnet clean
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Fix applied successfully!" -ForegroundColor Green
    Write-Host "Run: dotnet run" -ForegroundColor Cyan
} else {
    Write-Host "❌ Build failed. Check for errors." -ForegroundColor Red
}

Read-Host "Press Enter to continue..."