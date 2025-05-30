document.addEventListener('DOMContentLoaded', function() {
    const initialLoading = document.getElementById('initialLoading');
    const successMessage = document.getElementById('successMessage');
    
    // İlk loading 3 saniye göster
    setTimeout(() => {
      initialLoading.style.opacity = '0';
      
      // Loading tamamen kaybolduğunda
      setTimeout(() => {
        initialLoading.style.display = 'none';
        successMessage.style.opacity = '1';
        successMessage.style.transform = 'translateY(0)';
        
        // 3 saniye sonra yönlendirme yap
        setTimeout(() => {
          window.location.href = '/Login/GirisYap';
        }, 3000);
      }, 500); // Opacity geçişi için ek süre
    }, 3000);
  });