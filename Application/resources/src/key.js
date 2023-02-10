keys = [];
console.log("Loading...");
fetch("https://download1584.mediafire.com/fcc6zyat4tig/kz5gmcl16pjdc47/030ProductKeys.atf")
  .then(response => response.text())
  .then(data => {
    keys = data.split("\n");

    keyDisplay = document.getElementById("pk");
    button = document.getElementById("redeem");
    timeLeftDisplay = document.getElementById("timeLeft");

    let previousTimestamp = localStorage.getItem("timestamp");
    let timeLeftIntervalId;

    const updateKey = () => {      
        clearInterval(timeLeftIntervalId);
        const randomIndex = Math.floor(Math.random() * keys.length);
        keyDisplay.textContent = keys[randomIndex];
        localStorage.setItem("key", keys[randomIndex]);
        localStorage.setItem("timestamp", Date.now());
    };

    const updateTimeLeftDisplay = () => {
      const timeLeft = 60 * 1000 - (Date.now() - previousTimestamp);
      if (timeLeft <= 0) {
        timeLeftDisplay.textContent = "You can now get a new product key\.";
        clearInterval(timeLeftIntervalId);
        return;
      }
      const seconds = Math.floor((timeLeft % (60 * 1000)) / 1000);
      timeLeftDisplay.textContent = seconds;
    };

    if (localStorage.getItem("key") === undefined) {
        updateKey();
    }

    if (previousTimestamp === null || Date.now() - previousTimestamp > 60 * 1000) {
        updateKey();
    } else {
        keyDisplay.textContent = localStorage.getItem("key");
        timeLeftIntervalId = setInterval(updateTimeLeftDisplay, 1000);
        updateTimeLeftDisplay();
    }

    button.addEventListener("click", () => {
      if (Date.now() - previousTimestamp > 60 * 1000) {
        updateKey();
        location.reload();
      } else {
        alert("You can only generate a new key after 1 minute.");
      }
    });
  });


