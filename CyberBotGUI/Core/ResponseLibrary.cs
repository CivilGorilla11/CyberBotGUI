using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace CyberBotGUI.Core
{
    public static class ResponseLibrary
    {
        private static readonly ThreadLocal<Random> random =
    new ThreadLocal<Random>(() => new Random()); 

        public static string GetRandom(List<string> responses)
        {
            if (responses == null || responses.Count == 0)
                return "I am not so sure about that topic specifically, but I am always learning about cybersecurity related matters";

            return responses[random.Value.Next(responses.Count)];
        }

        public static readonly List<string> PasswordResponses = new List<string>
        {
            " A strong password is at least 12 characters long and mixes letters, numbers and symbols. Never reuse passwords across sites!",
            " Consider using a password manager like Bitwarden or 1Password. They generate and store complex passwords securely.",
            " Did you know 80% of data breaches involve weak or reused passwords? A unique password per account is your first defence.",
            " Enable two-factor authentication alongside your password for an extra security layer — even if your password leaks, your account stays safe."
        };

        // ── Phishing responses ──────────────────────────────
        public static readonly List<string> PhishingResponses = new List<string>
        {
            " Phishing emails often create urgency — 'Your account will be closed!' Always verify by going directly to the website, never via email links.",
            " Check the sender's email address carefully. Attackers use domains like 'paypa1.com' instead of 'paypal.com' to fool victims.",
            " Hover over links before clicking. If the URL looks suspicious or mismatched, do not click it — report it as phishing immediately.",
            " Legitimate companies will never ask for your password via email. If they do, it is almost certainly a phishing attempt."
        };

        // ── Scam responses ──────────────────────────────────
        public static readonly List<string> ScamResponses = new List<string>
        {
            " If an offer sounds too good to be true, it almost always is. Online scams often promise prizes, jobs or love to gain your trust.",
            " Never send money or gift cards to someone you have only met online. This is one of the most common romance and investment scam tactics.",
            " Phone scams often impersonate banks or government agencies. Hang up and call the official number directly to verify.",
            " Verify any charity or business before donating or transacting. Scammers create fake websites that look identical to real ones."
        };

        // ── Malware responses ───────────────────────────────
        public static readonly List<string> MalwareResponses = new List<string>
        {
            " Malware hides in email attachments, fake software downloads and infected USB drives. Only download software from official sources.",
            " Keep your antivirus updated and run regular scans. Modern malware can sit silently on your system for months before activating.",
            " Signs of malware infection include slow performance, unexpected popups, unknown programs and unusual network activity.",
            " Sandboxing suspicious files before opening them is a great habit — tools like Any.run let you safely analyse files online."
        };

        // ── Ransomware responses ────────────────────────────
        public static readonly List<string> RansomwareResponses = new List<string>
        {
            " Ransomware encrypts your files and demands payment for the key. The best protection is regular offline backups — then payment is never necessary.",
            " Never pay the ransom — payment does not guarantee file recovery and funds future attacks. Report it to authorities instead.",
            " Ransomware most commonly spreads through phishing emails and unpatched software. Keep systems updated and staff trained.",
            " The 3-2-1 backup rule protects against ransomware: 3 copies, 2 different media types, 1 stored offsite or in the cloud."
        };

        // ── Firewall responses ──────────────────────────────
        public static readonly List<string> FirewallResponses = new List<string>
        {
            " A firewall acts as a gatekeeper between your network and the internet, blocking unauthorised traffic based on security rules.",
            " Both hardware and software firewalls are important. Hardware protects the network perimeter while software protects individual devices.",
            " Regularly review your firewall rules. Outdated or overly permissive rules are a common entry point for attackers.",
            " Next-generation firewalls (NGFW) go beyond port filtering — they inspect traffic content, detect intrusions and apply application awareness."
        };

        // ── VPN responses ───────────────────────────────────
        public static readonly List<string> VpnResponses = new List<string>
        {
            " A VPN encrypts your internet connection and masks your IP address, making it much harder for attackers on public WiFi to intercept your data.",
            " Always use a VPN when connecting to public WiFi in cafes, airports or hotels. Your data travels through an encrypted tunnel.",
            " Not all VPNs are equal — avoid free VPNs that log your activity and sell your data. Look for a no-logs policy from a reputable provider.",
            " A VPN protects your privacy but is not a complete security solution. Combine it with antivirus and strong passwords for full protection."
        };

        // ── Hacker responses ────────────────────────────────
        public static readonly List<string> HackerResponses = new List<string>
        {
            " Not all hackers are criminals! Ethical hackers (penetration testers) are hired to find vulnerabilities before malicious actors do.",
            " Malicious hackers often spend weeks doing reconnaissance before attacking. Reducing your digital footprint limits what they can find.",
            " Bug bounty programmes pay researchers to responsibly disclose vulnerabilities. Companies like Google and Microsoft run these programmes.",
            " Social engineering is often easier than technical hacking — attackers manipulate people rather than systems. Awareness is your best defence."
        };

        // ── Encryption responses ────────────────────────────
        public static readonly List<string> EncryptionResponses = new List<string>
        {
            " Encryption converts your data into unreadable code that only authorised parties can decrypt. AES-256 is the current gold standard.",
            " Enable full-disk encryption on your devices. If your laptop is stolen, encrypted data is useless to the thief without your key.",
            " HTTPS uses TLS encryption to protect data between your browser and websites. Always check for the padlock icon before entering sensitive info.",
            "🗝 End-to-end encryption means only the sender and recipient can read messages — not even the service provider. Signal and WhatsApp use this."
        };

        // ── Backup responses ────────────────────────────────
        public static readonly List<string> BackupResponses = new List<string>
        {
            " Follow the 3-2-1 backup rule: 3 copies of your data, on 2 different media, with 1 stored offsite or in the cloud.",
            " Automate your backups so they run without you thinking about it. A backup you forget to run is no backup at all.",
            " Regularly test your backups by restoring files. Many organisations discover their backups were corrupt only when disaster strikes.",
            " Cloud backups protect against physical disasters like fire or theft. Combine cloud and local backups for maximum protection."
        };

        // ── Fallback responses ──────────────────────────────
        public static readonly List<string> FallbackResponses = new List<string>
        {
            " I am not sure I caught that. Could you rephrase? Try asking about topics like passwords, phishing, malware or VPNs.",
            " That is an interesting one! I specialise in cybersecurity — try asking me about encryption, firewalls, scams or backups.",
            " I did not quite get that. You can ask me about topics like hacking, ransomware, phishing or online safety.",
            " I am still learning! Try asking about a specific cybersecurity topic and I will do my best to help."
        };

        // ── Clarification responses ─────────────────────────
        public static readonly List<string> ClarificationResponses = new List<string>
        {
            " Could you tell me a bit more about what you mean? I want to make sure I give you the most relevant answer.",
            " Interesting — could you expand on that a little? The more context you give me, the better I can help.",
           " I want to make sure I understand correctly. Are you asking about protecting yourself, or understanding how attacks work?"
        };
    }
}
