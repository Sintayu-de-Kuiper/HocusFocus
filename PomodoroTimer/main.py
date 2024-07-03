import tkinter as tk
from tkinter import messagebox
import time
import subprocess

class PomodoroTimer:
    def __init__(self, root):
        self.root = root
        self.root.title("Pomodoro Timer with To-Do List and Distraction Blocker")

        self.timer_label = tk.Label(root, text="25:00", font=("Helvetica", 48))
        self.timer_label.pack()

        self.button_frame = tk.Frame(root)
        self.button_frame.pack()

        self.start_button = tk.Button(self.button_frame, text="Start", command=self.start_timer)
        self.start_button.pack(side=tk.LEFT)

        self.stop_button = tk.Button(self.button_frame, text="Stop", command=self.stop_timer)
        self.stop_button.pack(side=tk.LEFT)

        self.reset_button = tk.Button(self.button_frame, text="Reset", command=self.reset_timer)
        self.reset_button.pack(side=tk.LEFT)

        self.time_left = 25 * 60
        self.running = False

        # To-Do List
        self.todo_frame = tk.Frame(root)
        self.todo_frame.pack(pady=20)

        self.task_entry = tk.Entry(self.todo_frame, width=40)
        self.task_entry.pack(side=tk.LEFT, padx=5)

        self.add_task_button = tk.Button(self.todo_frame, text="Add Task", command=self.add_task)
        self.add_task_button.pack(side=tk.LEFT)

        self.task_listbox = tk.Listbox(root, width=50, height=10)
        self.task_listbox.pack(pady=10)

        self.list_buttons_frame = tk.Frame(root)
        self.list_buttons_frame.pack()

        self.remove_task_button = tk.Button(self.list_buttons_frame, text="Remove Selected Task", command=self.remove_task)
        self.remove_task_button.pack(side=tk.LEFT, padx=5)

        self.finish_task_button = tk.Button(self.list_buttons_frame, text="Finish Selected Task", command=self.finish_task)
        self.finish_task_button.pack(side=tk.LEFT, padx=5)

        # Distraction Blocker
        self.blocker_frame = tk.Frame(root)
        self.blocker_frame.pack(pady=20)

        self.block_entry = tk.Entry(self.blocker_frame, width=40)
        self.block_entry.pack(side=tk.LEFT, padx=5)

        self.add_block_button = tk.Button(self.blocker_frame, text="Add to Block List", command=self.add_to_block_list)
        self.add_block_button.pack(side=tk.LEFT)

        self.block_listbox = tk.Listbox(root, width=50, height=5)
        self.block_listbox.pack(pady=10)

        self.block_status_label = tk.Label(root, text="Distraction Blocker: OFF", fg="red")
        self.block_status_label.pack()

        self.blocked_items = []

    def update_timer(self):
        if self.running:
            if self.time_left > 0:
                mins, secs = divmod(self.time_left, 60)
                self.timer_label.config(text=f"{mins:02d}:{secs:02d}")
                self.time_left -= 1
                self.root.after(1000, self.update_timer)
            else:
                self.running = False
                self.timer_label.config(text="Time's up!")
                messagebox.showinfo("Pomodoro Timer", "Time's up! Take a break.")

                # Unblock websites after timer ends
                self.unblock_websites()
        else:
            # Unblock websites if timer stopped manually
            self.unblock_websites()

    def start_timer(self):
        if not self.running:
            self.running = True
            self.update_timer()
            self.block_websites()

            self.block_status_label.config(text="Distraction Blocker: ON", fg="green")

    def stop_timer(self):
        self.running = False
        self.timer_label.config(text="25:00")
        self.reset_blocker_status()

        # Unblock websites if timer stopped manually
        self.unblock_websites()

    def reset_timer(self):
        self.running = False
        self.time_left = 25 * 60
        self.timer_label.config(text="25:00")
        self.reset_blocker_status()

        # Unblock websites if timer reset
        self.unblock_websites()

    def add_task(self):
        task = self.task_entry.get()
        if task:
            self.task_listbox.insert(tk.END, task)
            self.task_entry.delete(0, tk.END)

    def remove_task(self):
        selected_task_index = self.task_listbox.curselection()
        if selected_task_index:
            self.task_listbox.delete(selected_task_index)

    def finish_task(self):
        selected_task_index = self.task_listbox.curselection()
        if selected_task_index:
            current_task = self.task_listbox.get(selected_task_index)
            self.task_listbox.delete(selected_task_index)
            self.task_listbox.insert(selected_task_index, current_task + " - Finished")
            self.task_listbox.itemconfig(selected_task_index, {'fg': 'gray', 'font': 'Helvetica 10 italic'})

    def add_to_block_list(self):
        website = self.block_entry.get()
        if website:
            self.block_listbox.insert(tk.END, website)
            self.blocked_items.append(website)
            self.block_entry.delete(0, tk.END)

    def block_websites(self):
        # Modify hosts file to block websites
        hosts_path = "/etc/hosts" if platform.system() == "Darwin" else "C:/Windows/System32/drivers/etc/hosts"
        redirect_ip = "127.0.0.1"

        with open(hosts_path, "r+") as file:
            content = file.read()
            for website in self.blocked_items:
                if website not in content:
                    file.write(redirect_ip + " " + website + "\n")

    def unblock_websites(self):
        # Restore original hosts file to unblock websites
        hosts_path = "/etc/hosts" if platform.system() == "Darwin" else "C:/Windows/System32/drivers/etc/hosts"

        with open(hosts_path, "r+") as file:
            lines = file.readlines()
            file.seek(0)
            for line in lines:
                if not any(website in line for website in self.blocked_items):
                    file.write(line)
            file.truncate()

    def reset_blocker_status(self):
        self.blocked_items.clear()
        self.block_listbox.delete(0, tk.END)
        self.block_status_label.config(text="Distraction Blocker: OFF", fg="red")

if __name__ == "__main__":
    import platform
    root = tk.Tk()
    timer = PomodoroTimer(root)
    root.mainloop()
