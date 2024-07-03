let timeLeft = 25 * 60;
let timerInterval;
let running = false;

function updateTimer() {
    let mins = Math.floor(timeLeft / 60);
    let secs = timeLeft % 60;
    document.getElementById("timer").textContent = `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
    if (timeLeft > 0) {
        timeLeft -= 1;
    } else {
        clearInterval(timerInterval);
        running = false;
        document.getElementById("timer").textContent = "Time's up!";
    }
}

function startTimer() {
    if (!running) {
        running = true;
        timerInterval = setInterval(updateTimer, 1000);
    }
}

function stopTimer() {
    clearInterval(timerInterval);
    running = false;
}

function resetTimer() {
    clearInterval(timerInterval);
    running = false;
    timeLeft = 25 * 60;
    document.getElementById("timer").textContent = "25:00";
}
