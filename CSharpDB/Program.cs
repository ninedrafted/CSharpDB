using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Data.SQLite;

namespace CSharpDB {
    class Program {
        static void Main(string[] args) {
            Test();
            DBTest cdb = new DBTest();
            Random r = new Random();
            for (int i = 0; i < 500; i++) {
                cdb.Add(r.Next(1000));
            }
            List<int> toRemove = new List<int>();
            for (int i = 0; i < 1000; i++) {
                toRemove.Add(r.Next(1000));
            }
            cdb.Add(5);
            cdb.Add(25);
            cdb.Add(7);
            cdb.Add(15);
            cdb.Add(18);
            cdb.Add(16);
            Console.WriteLine(cdb.Root.ToString());
            Console.WriteLine("IntegrityCheck:\t" + cdb.Root.IntegrityCheck());
            Console.WriteLine("WeightCheck:\t" + cdb.Root.WeightCheck());
            Console.ReadLine();
            Console.WriteLine("Select * WHERE int>15 AND int!=16 AND int!=45 AND int!=(some random numbers);");
            BTree<int> t = cdb.Root.GetGreaterAndEqual(15, false);
            Console.WriteLine("WeightCheck (>15):\t" + t.IntegrityCheck());
            t.RemoveAll(16);
            Console.WriteLine("WeightCheck (!16):\t" + t.IntegrityCheck());
            t.RemoveAll(45);
            Console.WriteLine("WeightCheck (!45):\t" + t.IntegrityCheck());
            for (int i = 0; i < toRemove.Count; i++) {
                t.RemoveAll(toRemove[i]);
            }
            Console.WriteLine("WeightCheck (!some random numbers):\t" + t.IntegrityCheck());
            List<DBIndex> list = t.GetSortedList(true);
            for (int i = 0; i < list.Count; i++) {
                Console.Write(cdb.Column0[list[i].A][list[i].B] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("WeightCheck:\t" + t.IntegrityCheck());
            Console.ReadLine();
            int[] v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.Write(v[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Select * WHERE int>=15");
            list = cdb.Root.GetGreaterAndEqual(15).GetSortedList();
            v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.Write(v[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Select * WHERE int>15 AND int <= 25");
            list = cdb.Root.GetGreaterAndEqual(15, false).GetLessAndEqual(25).GetSortedList();
            v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.WriteLine(v[i]);
            }
            Console.ReadLine();
        }
        static void Test() {
            string someText = "Indigenous people of the Everglades region From Wikipedia, the free encyclopedia Jump to: navigation, search Page move-protected This article is about the indigenous peoples of southern Florida. For indigenous peoples elsewhere in Florida see Indigenous peoples of Florida. A black and white photograph of four Seminole women and a child standing in front of a chickee wearing bright cotton Seminole patterns The Seminole family of Cypress Tiger in 1916 The indigenous people of the Everglades region arrived in the Florida peninsula of what is now the United States approximately 15,000 years ago, probably following large game. The Paleo-Indians found an arid landscape that supported plants and animals adapted to prairie and xeric scrub conditions. Large animals became extinct in Florida around 11,000 years ago. Climate changes 6,500 years ago brought a wetter landscape. The Paleo-Indians slowly adapted to the new conditions. Archaeologists call the cultures that resulted from the adaptations Archaic peoples. They were better suited for environmental changes than their ancestors, and created many tools with the resources they had. Approximately 5,000 years ago, the climate shifted again to cause the regular flooding from Lake Okeechobee that became the Everglades ecosystems. From the Archaic peoples, two major tribes emerged in the area: the Calusa and the Tequesta. The earliest written descriptions of these people come from Spanish explorers who sought to convert and conquer them. Although they lived in complex societies, little evidence of their existence remains today. The Calusa were more powerful in number and political structure. Their territory was centered around modern-day Ft. Myers, and extended as far north as Tampa, as far east as Lake Okeechobee, and as far south as the Keys. The Tequesta lived on the southeastern coast of the Florida peninsula around what is today Biscayne Bay and the Miami River. Both societies were well adapted to live in the various ecosystems of the Everglades regions. They often traveled through the heart of the Everglades, though they rarely lived within it. After more than 210 years of relations with the Spanish, both indigenous societies lost cohesiveness. Official records indicate that survivors of war and disease were transported to Havana in the late 18th century. Isolated groups may have been assimilated into the Seminole nation, which formed in northern Florida when a band of Creeks consolidated surviving members of pre-Columbian societies in Florida into their own to become a distinct tribe. Seminoles were forced into the Everglades by the U.S. military during the Seminole Wars from 1835 to 1842. The U.S. military pursued the Seminoles into the region, which resulted in some of the first recorded explorations of much of the area. Seminoles continue to live in the Everglades region, and support themselves with casino gaming on six reservations located throughout the state. Contents 1 Prehistoric peoples 2 Calusa 3 Tequesta 4 Seminole 5 See also 6 Bibliography 7 Notes and references 8 External links Prehistoric peoples Cultural Periods in Prehistoric South Florida[1] Period 	Dates Paleo-Indian 	10,000–7,000 BCE 500 BCE–800 CE Glades II 	800–1200 Glades III 	1200–1566 Historic 	1566–1763 Humans first inhabited the peninsula of Florida approximately 15,000 years ago when it looked vastly different. The west coast extended about 100 miles (160 km) to the west of its current location.[2] The landscape had large dunes and sweeping winds characteristic of an arid region, and pollen samples show foliage was limited to small stands of oak, and scrub bushes. As earth's glacial ice retreated, winds slowed and vegetation became more prevalent and varied.[3] The Paleo-Indian diets were dominated by small plants and the wild game available, which included saber-toothed cats, ground sloths, and spectacled bears.[4] The Pleistocene megafauna died out around 11,000 years ago.[5] Around 6,500 years ago, the climate of Florida began to change, and the land became much wetter. Paleo-Indians spent more time in camps and less time traveling in between sources of water.[6] The Paleo-Indians then slowly adapted and became the Archaic peoples of the Florida peninsula, most probably due to the extinction of big game. Archaic people were primarily hunter-gatherers who depended on smaller game and fish, and relied more than their predecessors on plants for food. They were able to adapt to the shifting climate and the resulting change of animal and plant populations. Florida experienced a prolonged drought at the onset of the Early Archaic era that lasted until the Middle Archaic period. Although the population decreased overall on the peninsula, the use of tools increased significantly during this time; artifacts have shown that these people used drills, knives, choppers, atlatls, and awls made from stone, antlers, and bone.[7] During the Late Archaic period, the climate became wetter again, and by approximately 3000 BCE, the rise of water tables allowed an increase in population and cultural development. Florida Indians formed into three similar but distinct cultures, Okeechobee, Caloosahatchee, and Glades, named for the bodies of water around which they were centered.[8] The Glades culture is divided into three periods based on evidence found in middens. In 1947, archaeologist John Goggin described the three periods after examining shell mounds. He excavated one on Matecumbe Key, another at Gordon Pass near modern-day Naples, and a third south of Lake Okeechobee near modern-day Belle Glade. The Glades I culture, lasting from 500 BCE to 800 CE, was apparently focused around Gordon Pass and is considered the least sophisticated due to the lack of artifacts. What has been found—primarily pottery—is gritty and plain.[9] With the advent of a well-established culture in 800 CE, the Glades II period is characterized by more ornate pottery, wide use of tools throughout the South Florida region, and the appearance of religious artifacts at burial sites. By 1200, the Glades III culture exhibited the height of their development. Pottery became ornate enough to be subdivided into types of decoration. More importantly, evidence of an expanding culture is revealed through the development of ceremonial ornaments made from shell, and the construction of large earthworks associated with burial rituals.[9] From the Glades III culture developed two distinct tribes that lived in and near the Everglades: the Calusa and the Tequesta. Calusa Main article: Calusa A color map of the lower portion of the Florida peninsula separated into three main regions Archaeological subareas of tribes that lived in and around the Everglades from 1513 to 1743[10] What is known of the inhabitants of Florida after 1566 was recorded by European explorers and settlers. Juan Ponce de León is credited as the first European to have contact with Florida's indigenous people in 1513. Ponce de León met with hostility from tribes that may have been the Ais and the Tequesta before rounding Cape Sable to meet the Calusa, the largest and most powerful tribe in South Florida. Ponce de León found at least one of the Calusa fluent in Spanish.[11] The explorer assumed the Spanish-speaker was from Hispaniola, but anthropologists have suggested that communication and trade between Calusa and native people in Cuba and the Florida Keys was common, or that Ponce de León was not the first Spaniard to make contact with the native people of Florida.[12] During his second visit to South Florida, Ponce de León was killed by the Calusa, and the tribe gained a reputation for violence, causing future explorers to avoid them.[13] In the more than 200 years the Calusa had relations with the Spanish, they were able to resist their attempts to missionize them. The Calusa were referred to as Carlos by the Spanish, which may have sounded like Calos, a variation of the Muskogean word kalo meaning \"black\" or \"powerful\".[14] Much of what is known about the Calusa was provided by Hernando de Escalante Fontaneda. Fontaneda was a 13-year-old boy who was the only survivor of a shipwreck off the coast of Florida in 1545. For seventeen years he lived with the Calusa until explorer Pedro Menéndez de Avilés found him in 1566. Menéndez took Fontaneda to Spain where he wrote about his experiences. Menéndez approached the Calusa with the intention of establishing relations with them to ease the settlement of the future Spanish colony. The chief, or cacique, was named Carlos by the Spanish. Positions of importance in Calusa society were given the adopted names Carlos and Philip, transliterated from Spanish royal tradition.[15] However, the cacique Carlos described by Fontaneda was the most powerful chief during Spanish colonization. Menéndez married his sister in order to facilitate relations between the Spanish and the Calusa.[16] This arrangement was common in societies in South Florida people. Polygamy was a method of solving disputes or settling agreements between rival towns.[17] Menéndez, however, was already married and expressed discomfort with the union. Unable to avoid the marriage, he took Carlos' sister to Havana where she was educated, and where one account reported that she died years later, the marriage never consummated.[18] A color photograph of an alligator head carved out of wood and painted, presented behind glass in a museum A Calusa wood carving of an alligator head excavated in Key Marco in 1895, on display at the Florida Museum of Natural History Fontaneda explained in his 1571 memoir that Carlos controlled fifty villages located on Florida's west coast, around Lake Okeechobee (which they called Mayaimi) and on the Florida Keys (they called Martires). Smaller tribes of Ais and Jaega who lived to the east of Lake Okeechobee, paid regular tributes to Carlos. The Spanish suspected the Calusa of harvesting treasures from shipwrecks and distributing the gold and silver between the Ais and Jaega, with Carlos receiving the majority.[19] The main village of the Calusa, and home of Carlos, bordered Estero Bay at present-day Mound Key where the Caloosahatchee River meets the Gulf of Mexico.[20] Fontaneda described human sacrifice as a common practice: when the child of a cacique died, each resident gave up a child to be sacrificed, and when the cacique died, his servants were sacrificed to join him. Each year a Christian was required to be sacrificed to appease a Calusa idol.[21] The building of shell mounds of varying sizes and shapes was also of spiritual significance to the Calusa. In 1895 Frank Hamilton Cushing excavated a massive shell mound on Key Marco that was composed of several constructed terraces hundreds of yards long. Cushing unearthed over a thousand Calusa artifacts. Among them he found tools made of bone and shell, pottery, human bones, masks, and animal carvings made of wood.[22] The Calusa, like their predecessors, were hunter-gatherers who existed on small game, fish, turtles, alligators, shellfish, and various plants.[23] Finding little use for the soft limestone of the area, they made most of their tools from bone or teeth, although they also found sharpened reeds effective. Weapons consisted of bows and arrows, atlatls, and spears. Most villages were located at the mouths of rivers or on key islands. They used canoes were used for transportation, as evidenced by shell mounds in and around the Everglades that border canoe trails. South Florida tribes often canoed through the Everglades, but rarely lived in them.[24] Canoe trips to Cuba were also common.[25] Calusa villages often had more than 200 inhabitants, and their society was organized in a hierarchy. Apart from the cacique, other strata included priests and warriors. Family bonds promoted the hierarchy, and marriage between siblings was common among the elite. Fontaneda wrote, \"These Indians have no gold, no silver, and less clothing. They go naked except for some breech cloths woven of palms, with which the men cover themselves; the women do the like with certain grass that grows on trees. This grass looks like wool, although it is different from it\".[26] Only one instance of structures was described: Carlos met Menéndez in a large house with windows and room for over a thousand people.[27] The Spanish found Carlos uncontrollable, as their priests and the Calusa fought almost constantly. Carlos was killed when a Spanish soldier shot him with a crossbow.[28] Following the death of cacique Carlos, leadership of the society passed to two caciques who were captured and killed by the Spanish.[15] Estimated numbers of Calusa at the beginning of the occupation of the Spanish ranged from 4,000 to 7,000.[29] The society endured a decline of power and population after Carlos; by 1697 their number was estimated to be about 1,000.[25] In the early 18th century, the Calusa came under attack from the Yamasee to the north; many asked to be removed to Cuba, where almost 200 died of illness. Some relocated to Florida,[30] and remnants may have been eventually assimilated into the Seminole culture, which developed during the 18th century.[31] Tequesta Main article: Tequesta Second in power and number to the Calusa in South Florida were the Tequesta (also called Tekesta, Tequeste, and Tegesta). They occupied the southeastern portion of the lower peninsula in modern-day Dade and Broward counties. They may have been controlled by the Calusa, but accounts state that they sometimes refused to comply with the Calusa caciques, which resulted in war.[20] Like the Calusa, they rarely lived within the Everglades, but found the coastal prairies and pine rocklands to the east of the freshwater sloughs habitable. To the north, their territory was bordered by the Ais and Jaega. Like the Calusa, the Tequesta societies centered around the mouths of rivers. Their main village was probably on the Miami River or Little River. A large shell mound on the Little River marks where a village once stood.[32] Though little remains of the Tequesta society, a site of archeological importance called the Miami Circle was discovered in 1998 in downtown Miami. It may be the remains of a Tequesta structure.[33] Its significance has yet to be determined, though archeologists and anthropologists continue to study it.[34] A black and white etching of Spanish explorer Pedro Menéndez de Avilés standing at a table with maps and holding a sword Pedro Menéndez de Avilés maintained a friendly relationship with the Tequesta. The Spanish described the Tequesta as greatly feared by their sailors, who suspected the natives of torturing and killing survivors of shipwrecks. Spanish priests wrote that the Tequesta performed child sacrifices to mark the occasion of making peace with a tribe with whom they had been fighting. Like the Calusa, the Tequesta hunted small game, but depended more upon roots and less on shellfish in their diets. They did not practice cultivated agriculture. They were skilled canoeists and hunted in the open ocean what Fontaneda described as whales, but were probably manatees. They lassoed the manatees and drove a stake through their snouts.[21][32] The first contact with Spanish explorers occurred in 1513 when Juan Ponce de León stopped at a bay he called Chequescha, or Biscayne Bay. Finding the Tequesta unwelcoming, he left to make contact with the Calusa. Menéndez met the Tequesta in 1565 and maintained a friendly relationship with them, building some houses and setting up a mission. He also took the chief's nephew to Havana to be educated, and the chief's brother to Spain. After Menéndez visited, there are few records of the Tequesta: a reference to them in 1673, and further Spanish contact to convert them.[35] The last reference to the Tequesta during their existence was written in 1743 by a Spanish priest named Father Alaña, who described their ongoing assault by another tribe. The survivors numbered only 30, and the Spanish transported them to Havana. In 1770 a British surveyor described multiple deserted villages in the region where the Tequesta had lived.[36] Archeologist John Goggin suggested that by the time European Americans settled the area in 1820, any remaining Tequesta were assimilated into the Seminole people.[32] Common descriptions of Native Americans in Florida by 1820 identified only the \"Seminoles\".[37] Seminole Main article: Seminole Following the demise of the Calusa and Tequesta, Native Americans in southern Florida were referred to as \"Spanish Indians\" in the 1740s, probably due to their friendlier relations with Spain. Between the Spanish defeat in the Seven Years' War in 1763 and the end of the American War of Independence in 1783, the United Kingdom ruled Florida. The term \"Seminolie\" was first used by a British Indian agent in a document dated 1771.[38] The beginnings of the tribe are vague, but records show that Creeks invaded the Florida peninsula, conquering and assimilating what was left of pre-Columbian societies into the Creek Confederacy. The mixing of cultures is evident in the language influences present among the Seminoles: various Muskogean languages, notably Hitchiti, and Creek, as well as Timucuan. In the early 19th century, a US Indian agent explained the Seminoles this way: \"The word Seminole means runaway or broken off. Hence ... applicable to all the Indians in the Territory of Florida as all of them ran away ... from the Creek ... Nation\".[39] Linguistically, the term \"Seminole\" comes from the Creek words Sua (Sun God), ma (mother, although in this connotation it is pejorative), and ol (people) to mean \"people whom the Sun God does not love\", or \"accursed\".[40] A black and white photograph of a Seminole man wearing traditional Seminole smock and vest, holding a rifle standing among palmettos, and staring at the viewer Seminoles such as Charlie Cypress, shown in 1900, have made their home in the Everglades. Creeks, who were centered in modern-day Alabama and Georgia, were known to incorporate conquered tribes into their own. Some Africans escaping slavery from South Carolina and Georgia fled to Florida, lured by Spanish promises of freedom should they convert to Catholicism, and found their way into the tribe.[41] Seminoles originally settled in the northern portion of the territory, but the 1823 Treaty of Moultrie Creek forced them to live on a 5-million-acre (20,000 km2) reservation north of Lake Okeechobee. They soon ranged farther south, where they numbered approximately 300 in the Everglades region,[42] including bands of Miccosukees—a similar tribe who spoke a different language—who lived in The Big Cypress.[43] Unlike the Calusa and Tequesta, the Seminole depended more on agriculture and raised domesticated animals. They hunted for what they ate, and traded with European-American settlers. They lived in structures called chickees, open-sided palm-thatched huts, probably adapted from the Calusa.[44] In 1817, Andrew Jackson invaded Florida to hasten its annexation to the United States in what became the First Seminole War. After Florida became a U.S. territory and settlement increased, conflicts between colonists and Seminoles became more frequent. The Second Seminole War (1835–1842) resulted in almost 4,000 Seminoles in Florida being displaced or killed. The Seminole Wars pushed the Indians farther south and into the Everglades. Those who did not find refuge in the Everglades were relocated to Oklahoma Indian territory under Indian Removal. The Third Seminole War lasted from 1855 to 1859. Over its course, 20 Seminoles were killed and 240 were removed.[43] By 1913, Seminoles in the Everglades numbered no more than 325.[45] They made their villages in hardwood hammocks, islands of hardwood trees that formed in rivers or pine rockland forests. Seminole diets consisted of hominy and coontie roots, fish, turtles, venison, and small game.[45] Villages were not large, due to the limited size of hammocks, which on average measured between one and 10 acres (40,000 m2). In the center of the village was a cook-house, and the largest structure was reserved for eating. When the Seminoles lived in northern Florida, they wore animal-skin clothing similar to their Creek predecessors. The heat and humidity of the Everglades influenced their adapting a different style of dress. Seminoles replaced their heavier buckskins with clothing of unique calico patchwork designs made of lighter cotton, or silk for more formal occasions.[46] The Seminole Wars increased the U.S. military presence in the Everglades, which resulted in the exploration and mapping of many regions that had not previously been recorded.[47] The military officers who had done the mapping and charting of the Everglades were approached by Thomas Buckingham Smith in 1848 to consult on the feasibility of draining the region for agricultural use.[48] Between the end of the Third Seminole War and 1930, the tribe lived in relative isolation. The construction of the Tamiami Trail, from 1928 to 1930, a road connecting Tampa to Miami and bisecting the Everglades, brought a steady stream of white people into Seminole territory that altered traditional ways of life. The Seminole began to work in local farms, ranches, and souvenir stands. They cleared land for the town of Everglades, and were \"the best fire fighters [the National Park Service] could recruit\" when Everglades National Park caught fire in times of drought.[49] As metropolitan areas in South Florida began to grow, the Seminoles became closely associated with the Everglades, simultaneously seeking privacy and serving as a tourist attraction, wrestling alligators and selling craftwork. As of 2008, there were six Seminole reservations throughout Florida; they feature casino gaming that support the tribe.[50]"; //out of en.wikipedia.org
            /*     someText = ". ";
                 Random r = new Random();
                 for (int i = 0; i < 2000; i++) {
                     if (i % 100 == r.Next(100))
                         someText += " to";
                     else someText += " " + r.Next();
                 }*/
            string[] splitted = someText.Split(' ');
            DB db1 = new DB("preceding", "itself", "following");
            DB db2 = new DB("preceding", "itself", "following");
            DB db3 = new DB("preceding", "itself", "following");

            SQLiteConnection sql1 = new SQLiteConnection();
            sql1.ConnectionString = "Data Source=:memory:";
            sql1.Open();
            SQLiteCommand command = new SQLiteCommand(sql1);
            command.CommandText = "CREATE TABLE IF NOT EXISTS sql1 ( id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, preceding TEXT, itself TEXT, following TEXT );";
            command.ExecuteNonQuery();
            Stopwatch s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 1; i < splitted.Length - 1; i++) {
                db1.Add(false, splitted[i - 1], splitted[i], splitted[i + 1]);
            }
            s.Stop();
            Console.WriteLine("Adding to DB1 without optimisation " + (splitted.Length - 2) + " times 3 strings lasted: " + s.Elapsed);
            s = new Stopwatch();
            splitted = someText.Split(' ');
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 1; i < splitted.Length - 1; i++) {
                db2.Add(false, splitted[i - 1], splitted[i], splitted[i + 1]);
            }
            s.Stop();
            Console.WriteLine("Adding to DB2 without optimisation " + (splitted.Length - 2) + " times 3 strings lasted: " + s.Elapsed);
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 1; i < splitted.Length / 10 - 1; i++) {
                db3.Add(true, splitted[i - 1], splitted[i], splitted[i + 1]);
            }
            s.Stop();
            Console.WriteLine("Adding to DB3 with optimisation " + (splitted.Length / 10 - 2) + " times 3 strings lasted: " + s.Elapsed);
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 1; i < splitted.Length - 1; i++) {
                command = new SQLiteCommand(sql1);
                command.CommandText = "INSERT INTO sql1 (preceding, itself, following) VALUES(@p1, @p2, @p3);";
                command.Parameters.Add(new SQLiteParameter("@p1", splitted[i - 1]));
                command.Parameters.Add(new SQLiteParameter("@p2", splitted[i]));
                command.Parameters.Add(new SQLiteParameter("@p3", splitted[i + 1]));
                command.ExecuteNonQuery();
            }
            s.Stop();
            Console.WriteLine("Adding to SQL1 " + (splitted.Length - 2) + " times 3 strings lasted: " + s.Elapsed);
            s = new Stopwatch();
            Console.WriteLine("DB1 WeightCheck: " + db1.WeightCheck());
            Console.WriteLine("DB2 WeightCheck: " + db2.WeightCheck());
            Console.WriteLine("DB3 WeightCheck: " + db3.WeightCheck());
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            db2.UpdateWhere("itself", "to", "toto");
            s.Stop();
            Console.WriteLine("Replacing in DB2 \"to\" with \"toto\" 1 times lasted: " + s.Elapsed);
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            command = new SQLiteCommand(sql1);
            command.CommandText = "UPDATE sql1 SET itself='toto' WHERE itself=='to';";
            command.ExecuteNonQuery();
            s.Stop();
            Console.WriteLine("Replacing in SQL1 \"to\" with \"toto\" 1 times lasted: " + s.Elapsed);

            List<List<IComparable>> gets = new List<List<IComparable>>();
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 0; i < 500; i++) {
                gets.AddRange(db1.GetWhere("itself", "is"));
                gets.AddRange(db1.GetWhere("itself", "to"));
            }
            s.Stop();
            Console.WriteLine("Getting of DB1 in 1000 request " + gets.Count + " rows lasted:" + s.Elapsed);
            gets = new List<List<IComparable>>();
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 0; i < 500; i++) {
                gets.AddRange(db2.GetWhere("itself", "is"));
                gets.AddRange(db2.GetWhere("itself", "to"));
            }
            s.Stop();
            Console.WriteLine("Getting of DB2 in 1000 request " + gets.Count + " rows lasted:" + s.Elapsed);
            gets = new List<List<IComparable>>();
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 0; i < 500; i++) {
                gets.AddRange(db3.GetWhere("itself", "is"));
                gets.AddRange(db3.GetWhere("itself", "to"));
            }
            s.Stop();
            Console.WriteLine("Getting of DB3 in 1000 request " + gets.Count + " rows lasted:" + s.Elapsed);
            List<SQLiteDataReader> readers = new List<SQLiteDataReader>();
            s = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            s.Start();
            for (int i = 0; i < 500; i++) {
                command = new SQLiteCommand(sql1);
                command.CommandText = "SELECT preceding, itself, following FROM sql1 WHERE itself=='is' OR itself=='to';";
                readers.Add(command.ExecuteReader(System.Data.CommandBehavior.Default));
            }
            s.Stop();
            TimeSpan elapsed = s.Elapsed;
            System.Threading.Thread.Sleep(100);
            s = new Stopwatch();
            s.Start();
            int sqlcount = 0;
            for (int i = 0; i < readers.Count; i++) {
                //Console.WriteLine(nvc[0] + " " + nvc[1] + " " + nvc[2]);

                while (!readers[i].IsClosed && readers[i].Read()) sqlcount++;
            }
            command = new SQLiteCommand(sql1);
            command.CommandText = "SELECT count(itself) FROM sql1 WHERE itself=='is' OR itself=='to';";
            //sqlcount = System.Convert.ToInt32(command.ExecuteScalar()) * 500;
            s.Stop();
            Console.WriteLine("Getting of SQL1 in 500 request " + sqlcount + " rows lasted:" + elapsed + " (" + s.Elapsed + ")");
            Console.ReadLine();
        }
    }
}