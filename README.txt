Stack Attack – Space Shooter (Unity)

Unity sürümü: 6000.0.58f2
Platform: 2D (mobil odaklı), PC’de fare ile test edilebilir
Referans oyun: Stack Attack (Google Play) → bu projede mekaniği “uzay/şooter” temasına uyarlanmıştır.



Özellikler

Başlangıç → Oynanış → Tamamlandı ekran akışı (Tap to Start / Level Completed).

5 seviyelik içerik, her seviyede süre (örn. 30 sn) ve farklı düşman setleri.

Kaldığı yerden devam: Uygulama kapanıp açılsa bile son seviyeden başlar.

Oyuncu hareketi: Esas olarak sağ–sol, sınırlı bir dikey bantta hareket.

Ateş Mekaniği: Sağ tık (PC) / mobilde dokunma varyasyonlarıyla tekli atış.

Sağlık Sistemi: Oyuncu 4 can; farklı meteorlar farklı hasar verir.

Rastgele sağlık paketi düşüşleri (can eksikken).

Boss savaşı: Son seviyede tek boss; 3 namludan ateş, yatay devriye, 10 HP.

Ses & VFX: Atış, vurulma, patlama, UI tıklamaları için SFX/VFX kancaları.

Tam durdurma: Süre başlamadan ve bittiğinde oyun/ hareket/ateş kilitli.



Kurulum

Unity 6000.0.58f2 ile açın.

Project Settings > Editor > Version Control ayarlarını gerekirse güncelleyin.

Aspect: Game view’da Android telefon oranı (ör. 1080×1920, Portrait).

Not: Ücretsiz third-party ya da Unity default asset’leri kullanılabilir.



Oynanış ve Kontroller

Başlangıç: “Tap to Start” paneline dokun/ tıkla.

Hareket: Ekranda sürükle (PC’de sol tık basılı tut). Oyuncu daha çok yatay hareket eder; dikeyde küçük bir bantta dolaşır.

Ateş: PC’de sağ tık.

Debug kısayolları (opsiyonel):

R: seviyeyi 1’e sıfırla ve sahneyi yeniden yükle.

T: Tap paneli göster.

Y: Timer’ı başlat (debug).



Oyun Akışı ve Ekranlar

Start (Tap Panel): Süre başlamadan oyun durur (Time.timeScale=0). Tıklayınca süre başlar, oyun akar.

Gameplay: Level süresi çalışır, düşmanlar spawn olur, oyuncu ateş eder.

Complete: Süre dolunca tam durur (hareket/mermiler/AI durur). “Level Completed” paneli açılır → tıklayınca bir sonraki seviyeye geçilir. Son seviyede boss ölünce panel açılır.

Seviye Tasarımı

Minimum 5 seviye. Her seviyede:

Süre (LevelTimer ile).

Düşman listesi & spawn aralığı (LevelManager + Spawner).

Arka plan (BackgroundSwitcher → Sprite[]).

Son seviye: Boss Level. Yalnızca tek boss üretilir, normal spawner çalışmaz.



Sahne ve Inspector Kurulumu

Sahneler:

Start (opsiyonel) → menü.

Game → ana oyun sahnesi.

Hierarşi
Game
 ├─ Canvas
 │   ├─ TapPanel (Button + Text "Tap to Start")
 │   ├─ HUD (Health Slider, Timer Slider, Level Label)
 │   └─ CompletePanel (Button + Text "LEVEL X COMPLETED")
 ├─ Player (SpriteRenderer, Collider2D, PlayerControllerFull, PlayerShooting, PlayerHealth)
 ├─ Spawner (Spawner.cs)
 ├─ PickupSpawner (PickupSpawner.cs)
 ├─ Background (SpriteRenderer + FitToCamera2D + BackgroundSwitcher)
 ├─ GameOverUI (Panel ataması, butonlar)
 └─ LevelManager (bu sahneyi yönetir)



Önemli Tag/Layer:

Player objesi: Tag = Player.

Physics2D Layer matriksi: PlayerBullets ↔ Enemies çarpışmaları açık olmalı.

Boss Kurulumu:

Boss prefab içinde:

SpriteRenderer, Rigidbody2D (Kinematic, Gravity 0), PolygonCollider2D (isTrigger=ON).

EnemyHealth (maxHP=10) (patlama SFX/VFX burada).

BossController (yatay devriye, ekrana oturtma).

Muzzle_L / Muzzle_M / Muzzle_R boş child Transform’lar (namlu uçları).

BossShooter → firePoints dizisine Muzzle’ları sırayla ata, EnemyBullet prefabi ver.

(İsteğe bağlı) BossDamage → vurulma SFX/flash/VFX.

Önemli Scriptler

LevelManager

Oyun akışını ve UI’ı yönetir, global oynanabilirlik bayrağı: public static bool IsPlaying.

Timer bitince ve tap panel açıkken tam durdurur (Time.timeScale=0).

Son seviyede boss’u tek kez üretir; boss ölünce Level Completed açılır.

LevelTimer

Seviye süresini yönetir, Slider ve kalan saniye metni günceller.

Spawner

Düşman prefablardan rastgele üretir. Boss seviyesinde kullanılmaz.

PlayerControllerFull

Sağ–sol hareket, dar dikey bant, thruster kontrolü.

if (!LevelManager.IsPlaying) return; → oyun akmıyorsa hareket yok.

PlayerShooting

Sağ tıkla ateş. IsPlaying değilse ateş yok.

Bullet / EnemyBullet

RigidBody2D (Continuous), Collider2D (isTrigger=ON).

Bullet: EnemyHealth varsa hasar uygular (boss/düşman), yoksa meteor gibi objeyi yok eder.

PlayerHealth

4 can, hasar alma, i-frame, ölümde SFX/VFX ve GameOver kancası.

DamageDealer

Çarpışmada uygulanacak hasar değeri (meteor vb.).

PickupSpawner & HealthPickup

Rastgele aralıklarla can paketi düşürür (oyuncu canı dolu değilse).

BackgroundSwitcher + FitToCamera2D

Her level için farklı arka plan ve ekrana sığdırma.

GameOverUI

Ölüm ekranı, Restart/GoHome/Quit.




Kaydetme / Devam Etme

Son seviye PlayerPrefs ile saklanır:

Anahtar: last_level

R tuşu ile 1. seviyeye sıfırlanır.