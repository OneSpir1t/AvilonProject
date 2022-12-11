<!DOCTYPE html>
<html lang="ru">

<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" href="css/style.css" type="text/css"/>
    <title>Avilon</title>
</head>
<body>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).on('change', '#select1', function(){
            $BrandSel = $('option:selected', this).text();
            $.ajax({
                url: "index.php",
                type: "POST",
                data: $BrandSel,
                success: function(data){
                    $("input.Brand").val($BrandSel);
                }          
            })
        })
        $(document).on('change', '#select2', function(){
            $BodyTypeSel = $('option:selected', this).text();
            $.ajax({
                url: "index.php",
                type: "POST",
                data: $BodyTypeSel,
                success: function(data){
                    $("input.BodyType").val($BodyTypeSel);
                }          
            })
        })
        $(document).on('change', '#select3', function(){
            $TypeEngineSel = $('option:selected', this).text();
            $.ajax({
                url: "index.php",
                type: "POST",
                data: $TypeEngineSel,
                success: function(data){
                    $("input.TypeEngine").val($TypeEngineSel);
                }          
            })
        })
        $(document).on('change', '#select4', function(){
            $ColorSel = $('option:selected', this).text();
            $.ajax({
                url: "index.php",
                type: "POST",
                data: $ColorSel,
                success: function(data){
                    $("input.Color").val($ColorSel);
                }          
            })
        })
    </script>
    </script>
    <?php
        require_once 'connection.php';
        $Equipments = Update_CarList($conn);
        $Brands = $conn->query("Select * from Brands");
        $BodyTypes = $conn->query("Select * from Bodytypes");
        $EngineTypes = $conn->query("Select * from EngineTypes");
        $Colors = $conn->query("Select * from Colors");
        function get_Color($Equipment, $conn)
        {
            $str = $conn->prepare("select colors.Id, colors.Title from colors where Id in (Select technicalinformation.ColorID from technicalinformation inner join equipments on equipments.TechnicalInformationID = technicalinformation.ID where equipments.Id = ?)");
            $str->execute([$Equipment["ID"]]);
            while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                return $ab;
            } 
        }     
        function Update_CarList($conn)
        {
            return $Equipments = $conn->query("Select * from Equipments");
        } 
    ?>
    <div class="topmenu" style="background-color:#221E1F;">
        <div class="mainlogo">
            <a href="#" class="mainlogo">
                <img class="logo" src="img/logo.png" style="max-height:150px;">		
            </a>
        </div>
        
        <div class="rightNAVpanel">
            <a class="text-title" href="#" role="menuitem" style="color:#ffffff; padding: 0 15px 0 15px; font-weight: 600; font-size:60px;">О нас</a>
            <a class="text-title" href="#" role="menuitem" style="color:#ffffff; padding: 0 15px; font-weight: 600; font-size:60px;">Каталог</a>
            <a class="text-title" href="#" role="menuitem" style="color:#ffffff; padding: 0 15px 0 15px; font-weight: 600; font-size:60px;">Контакты</a>
        </div>
    </div>
    <div class="Mainblock">
        <div class="PageInfo">
            <span>
                <a href="#" title="Главная">
                    <img src="./img/home.svg">
                    <span>Главная</span>
                </a>
            </span>
        </div>
        <div class="leftsortpanel" style="background-color: #f8f8f8;">
            <div class="Selector">
                <label>Марка: </label>
                <select id="select1" class="MarkSelector">
                    <option value="0">Все</option>
                    <?php        
                        while($Brand = $Brands->fetch()){?>
                            echo("<option value="<?php echo $Brand["Title"] ?>"><?php echo $Brand["Title"] ?></option>");
                    <?php } ?>
                </select>
                <input type="hidden" class="Brand" value="Все">
            </div>
            <div class="Selector">
                <label>Тип Кузова: </label>
                <select id="select2" class="BodyTypeSelector">
                    <option value="ad">Все</option>
                    <?php        
                        while($BodyType = $BodyTypes->fetch()){?>
                            echo("<option><?php echo $BodyType["Title"] ?></option>");
                    <?php } ?>
                </select>
                <input type="hidden" class="BodyType" value="Все">
            </div>
            <div class="Selector">
                <label>Тип двигателя: </label>
                <select id="select3" class="TypeEngineSelector">
                    <option value="0">Все</option>
                    <?php        
                        while($EngineType = $EngineTypes->fetch()){?>
                            echo("<option><?php echo $EngineType["Title"] ?></option>");
                    <?php } ?>
                </select>
                <input type="hidden" class="TypeEngine" value="Все">
            </div>
            <div class="Selector">
                <label>Цвет: </label>
                <select id="select4" class="ColorSelector">
                    <option value="0">Все</option>
                    <?php        
                        while($Color = $Colors->fetch()){?>
                            echo("<option><?php echo $Color["Title"] ?></option>");
                    <?php } ?>
                </select>
                <input type="hidden" class="Color" value="Все">
            </div>
        </div>
        <div class="Carlist">
            <?php        
                while($Equipment = $Equipments->fetch()){?>
                    <?php
                        if($Equipment["Image"] == null){$Equipment["Image"] = "defaultImage.png";}
                    ?>
                    <div class="CarItem">
                        <img class="CarImg" src = "CarImages/<?php echo $Equipment["Image"] ?>">
                        <div class="DescriptionCars">
                            <label>
                                <?php
                                    $str = $conn->prepare("select brands.title from brands where Id in (Select BrandId from models inner join equipments on models.Id = equipments.ModelID where equipments.Id = ? )");
                                    $str->execute([$Equipment["ID"]]);
                                    while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                                        echo $ab["title"];
                                    }                                                       
                                ?>
                            </label>
                            <label>
                                <?php
                                    $str = $conn->prepare("select models.title from models inner join equipments on equipments.ModelID = Models.Id where equipments.ID = ?; )");
                                    $str->execute([$Equipment["ID"]]);
                                    while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                                        echo $ab["title"];
                                    } 
                                ?>
                            </label>
                            <label>
                                <?php 
                                    $str = $conn->prepare("select bodytypes.Title from bodytypes where Id in (Select technicalinformation.BodyTypeID from technicalinformation inner join equipments on equipments.TechnicalInformationID = technicalinformation.ID where equipments.Id = ?)");
                                    $str->execute([$Equipment["ID"]]);
                                    while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                                        echo $ab["Title"];
                                    } 
                                 ?>                            
                            </label>
                            <div class="iconsCar">               
                                <span>
                                    <img src="./img/engine.svg">
                                    <span>
                                        <?php 
                                        $str = $conn->prepare("select technicalinformation.Horsepower from technicalinformation inner join equipments on equipments.TechnicalInformationID = technicalinformation.ID where equipments.ID = ?");
                                        $str->execute([$Equipment["ID"]]);
                                        while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                                            echo $ab["Horsepower"]. " л.с." ;
                                        } 
                                        ?>  
                                    </span>
                                </span>                       
                            </div>
                            <label>Комплектация: <?php echo $Equipment["Title"] ?></label>
                            <div class="iconsCar">
                            <img src="./img/fuel.svg">
                                <label>
                                    <?php 
                                    $str = $conn->prepare("select enginetypes.Title from enginetypes where Id in (Select technicalinformation.EngineTypeID from technicalinformation inner join equipments on equipments.TechnicalInformationID = technicalinformation.ID where equipments.Id = ?)");
                                    $str->execute([$Equipment["ID"]]);
                                    while($ab = $str->fetch(PDO::FETCH_ASSOC)){
                                        echo $ab["Title"];
                                    } 
                                    ?>  
                                </label>
                            </div>    
                            <label class="colorCar">
                                <span class="spanColorCar" style="background-color:<?php switch(get_Color($Equipment, $conn)["Id"]){case 1: echo "green";break; case 2: echo "red";break;case 3: echo "white";break;case 4: echo "orange";break;case 5: echo "yellow";break;case 6: echo "black";break;case 7: echo "blue";break;case 8: echo "gray";break;} ?>"></span>
                                <?php 
                                    echo(get_Color($Equipment, $conn)["Title"]); 
                                ?>                        
                            </label>    
                            <label>Цена: <?php echo $Equipment["Cost"] ." ₽"?></label>
                        </div>
                    </div>
            <?php } ?>                    
        </div>
    </div>
</body>

</html>